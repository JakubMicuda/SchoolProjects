/**
 * @author Jakub Micuda (433715)
 * @file main.c
 **/


#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <strings.h>
#include <dirent.h>
#include <pwd.h>
#include <sys/stat.h>
#define ARGerr {fprintf(stderr,"invalid argument\n"); exit(1);}
#define FILerr fprintf(stderr,"cannot read file\n");

typedef enum{DEF,F,S,E,SE,ES} SORT;

typedef struct file{
        char *path,*name,*ext;
        unsigned long int size;
}file_t;

typedef struct finder{
    char* name, *extension, *user, *root;
    SORT sort_type;
    int hiddenEnable, foundCount;
}FIND;

int sort_by_path(const void *a,const void *b);
int sort_by_name(const void *a, const void *b);
int sort_by_extension(const void *a, const void *b);
int sort_by_size(const void *a, const void *b);
int sort_ES(const void *a, const void *b);
int sort_SE(const void *a, const void *b);

void choose_sort(SORT type, int arr_size, file_t **files);
void fileFinding(FIND *item, file_t **files);
void initFIND(FIND *item, int count, char* argument[]);
void freeFinder(FIND item);
void freeFile(file_t *item);
void print(file_t **files, int count);

SORT stringCheck(char* string);
int isArgument(char* string);
file_t* checkFile(struct dirent *file, FIND *item, struct stat *statEntry, file_t **files);
char *strdup(const char *str);

/**
 * @brief program zoberie vstupne parametre, skontroluje ich validitu a nacita do struktury,
 *        potom necha vstupnu cestou prejst funkciou na prechadzanie suborov a najde vyhovujuce
 *        nakoniec ich zoradi, vypise na vystup a uvolni alokovanu pamet
 * @param argc, pocet vstupnych parametrov
 * @param argv[], pole vstupnych argumentov
 **/
int main(int argc, char* argv[])
{
    FIND input;
    input.foundCount = 0;
    initFIND(&input,argc,argv);
    file_t **files = malloc(3500*sizeof(file_t*));
    fileFinding(&input, files);
    choose_sort(input.sort_type,input.foundCount,files);
    print(files,input.foundCount);
    free(files);
    free(input.root);
    return 0;
}

 /**
  * @brief na vstup berie string, ktory zduplikuje, alokuje pamet a vrati ho na vystup
  * @param str, string ktory sa ide duplikovat
  * @return ukazatel na novy string totozny so vstupnym
  */
char *strdup(const char *str){
    if(str == NULL){
        fprintf(stderr,"ERROR - Function strdup failed: Argument is NULL.\n");
        return NULL;
    }
    char *dup = calloc(strlen(str) + 1,sizeof(char));
    if(dup)
        strcpy(dup, str);
    else{
        fprintf(stderr,"ERROR - Function strdup failed: Allocation failed.\n");
        return NULL;
    }
    return dup;
}

/**
 * @brief funkcia zoberie na vstup vstupnu strukturu, pocet argumentov a ich pole a skontroluje validitu.
 *        potom inicializuje prvky danej struktury podla nich
 * @param item, vstupna struktura
 * @param count, pocet argumentov
 * @param argument[], pole argumentov
 */
void initFIND(FIND *item, int count, char* argument[])
{
    item->name = item->extension = item->user = NULL;
    item->hiddenEnable = 0;
    item->sort_type = DEF;
    int i=1;
    if(count<2) item->root = strdup(".");
    else if(argument[1][0]=='-')
        {
            item->root = strdup(".");
        }else{
            item->root = strdup(argument[1]);
            i++;
        }
    DIR *dir = NULL;
    if(!(dir = opendir(item->root)))
	{
	   FILerr
	   exit(2);
	}
    closedir(dir);
    while(i<count)
    {
        if(strlen(argument[i])!=2)
	{
	    free(item->root);
	    ARGerr
	}

        switch(argument[i][1])
        {
            case 'n': {     if(i+1==count){ free(item->root); ARGerr}
                            item->name = argument[i+1];
                            i++;
                            break;}

            case 'e': {     if(i+1==count) { free(item->root); ARGerr}
                            item->extension = argument[i+1];
                            i++;
                            break;}

            case 'u': {     if(i+1==count){ free(item->root); ARGerr}
                            item->user = argument[i+1];
                            i++;
                            break;}

            case 'a': { item->hiddenEnable = 1; break;}

            case 's': { if(i+1==count){ free(item->root); ARGerr }
                        if(isArgument(argument[i+1])) ARGerr
                        item->sort_type = stringCheck(argument[i+1]);
                        if(item->sort_type==DEF){ free(item->root); ARGerr}
                        i++;
                        break;}

            case 'h': {   printf("-n ATTR -> Hladanie na zaklade podretazca v mene suboru, kde ATTR je podretazec\n");
                          printf("-e EXT -> Hladanie na zaklade pripony suboru (xml,txt,c,h,...)\n");
                          printf("-s \"f\" | \"s\" | \"e\" | \"se\", \"es\"  -> Triedenie vypisu ciest k suborom na zaklade celej cesty, velkosti, pripony, velkosti a pripony, pripony a velkosti\n");
                          printf("-u USER -> Hladanie suboru na zaklade mena pouzivatela, kde USER je pouzivatelovo meno\n");
                          printf("-a -> Hlada aj v skryte subory a podadresare\n");
			  free(item->root);
                          exit(0);}

            default: {free(item->root); ARGerr }
        }
        i++;
    }
}

 /**
  * @brief podla vstupneho stringu porovnava s roznymi parametrami a podla toho vrati vyctovy typ
  * @param string, vstupny string
  * @return vrati vyctovy typ
  */
SORT stringCheck(char* string)
{
    if(!strcmp(string,"f")) return F;
    if(!strcmp(string,"s")) return S;
    if(!strcmp(string,"e")) return E;
    if(!strcmp(string,"es")) return ES;
    if(!strcmp(string,"se")) return SE;
    return DEF;
}

 /**
  * @brief skontroluje ci sa zhoduje vstupny string s jednym s parametrov
  * @param string, vstupny string
  * @return vrati 1 ak sa sniektorym zhoduje, inak 0
  */
int isArgument(char* string)
{
    if(!strcmp(string,"-n")) return 1;
    if(!strcmp(string,"-e")) return 1;
    if(!strcmp(string,"-u")) return 1;
    if(!strcmp(string,"-s")) return 1;
    if(!strcmp(string,"-a")) return 1;
    if(!strcmp(string,"-h")) return 1;
    return 0;
}

/**
 * @brief funkcia prehladava danu cestu a uklada subory ktore vyhovuju podmienkam do pola struktur
 * @param item, vstupna struktura
 * @param files, pole struktur
 */
void fileFinding(FIND *item, file_t **files)
{
    DIR *dir = NULL;
    int dlzka = strlen(item->root);
    if ((dir = opendir(item->root))) {
        struct dirent *dirEntry = NULL;
        struct stat *statEntry = NULL;
        statEntry = malloc(sizeof(struct stat));
        while ((dirEntry = readdir(dir)) != NULL) {
            item->root = realloc(item->root,sizeof(char)*(dlzka+strlen(dirEntry->d_name)+2));
            if(item->root[strlen(item->root)-1] != '/')
                strcat(item->root,"/");
            strcat(item->root,dirEntry->d_name);

            if(strcmp(dirEntry->d_name,"..") &&  strcmp(dirEntry->d_name,"."))
                {
                    if(dirEntry->d_type==8)
                    {
                            stat(item->root,statEntry);
                            files[item->foundCount] = checkFile(dirEntry, item, statEntry, files);

                    }else if(dirEntry->d_type==4)
                    {
                        if((item->hiddenEnable && dirEntry->d_name[0]=='.') || dirEntry->d_name[0] != '.') fileFinding(item, files);
                    }

                }
             item->root[strlen(item->root) - strlen(dirEntry->d_name) - 1] = '\0';
    }
     free(statEntry);
     free(dirEntry);
     closedir(dir);
  }else FILerr

}

 /**
  * @brief funkcia kontroluje, ci dany subor sedi s podmienkami vstupu, ak ano ulozi do struktury uzitocne data o nom
  * @param file, struktura suboru
  * @param statEntry, struktura obsahujuca hlavne meno vlastnika suboru
  * @param item, vstupna struktura
  * @param files, pole struktur vyhovujucich suborov
  */
file_t* checkFile(struct dirent *file, FIND *item, struct stat *statEntry, file_t **files)
{
    char *ext, *name;
    file_t *pom;

    if(file->d_name[0] == '.' && item->hiddenEnable == 0) return files[item->foundCount];

    ext=name=NULL;
    ext = strrchr(file->d_name,'.');
        if(ext == NULL || ext == file->d_name){
            name = strdup(file->d_name);
            ext = NULL;
        }else{
        ext = ext + 1;

        name = (char *) calloc (sizeof(char),strlen(file->d_name)-strlen(ext));
        memcpy(name,file->d_name,sizeof(char)*strlen(file->d_name)-strlen(ext)-1);
        }
    if(item->extension!=NULL)
    {
        if(ext == NULL) return files[item->foundCount];
        if(strcmp(item->extension,ext)){
            free(name);
            return files[item->foundCount];
        }
    }

    if(item->name != NULL)
    {
        if(strstr(name,item->name)==NULL){
            free(name);
            return files[item->foundCount];
        }
    }

    if(item->user!=NULL)
    {
        if(strcmp(item->user,getpwuid(statEntry->st_uid)->pw_name)){
            free(name);
            return files[item->foundCount];
        }
    }

    pom = malloc(sizeof(file_t));
    if(ext != NULL) pom->ext = strdup(ext);
    else pom->ext = NULL;
    pom->path = strdup(item->root);
    pom->name = strdup(name);
    pom->size = statEntry->st_size;
    item->foundCount++;
    free(name);
    return pom;
}

 /**
  * @brief funkcia uvolnuje pamet zo vstupnej struktury
  * @param item, vstupna struktura
  */
void freeFile(file_t *item)
{
    free(item->ext);
    free(item->name);
    free(item->path);
    free(item);
}

 /**
  * @brief funkcia triedi subory podla cesty
  */
int sort_by_path(const void *a, const void *b)
{
    file_t *fst = *(file_t**)a;
    file_t *snd = *(file_t**)b;
    return strcmp(fst->path,snd->path);
}

 /**
  * @brief funkcia triedi subory podla mena
  */
int sort_by_name(const void *a, const void *b)
{
    file_t *fst = *(file_t**)a;
    file_t *snd = *(file_t**)b;
    int compare = strcasecmp(fst->name,snd->name);
    if(compare) return compare;
    else return sort_by_path(a,b);
}

 /**
  * @brief funkcia triedi subory podla velkosti
  */
int sort_by_size(const void *a, const void *b)
{
    file_t *fst = *(file_t**)a;
    file_t *snd = *(file_t**)b;
    if(fst->size == snd->size) return sort_by_name(a,b);
    else if(fst->size < snd->size) return 1;
    else return -1;
}

  /**
  * @brief funkcia triedi subory podla pripony
  */
int sort_by_extension(const void *a, const void *b)
{
    file_t *fst = *(file_t**)a;
    file_t *snd = *(file_t**)b;
    if(fst->ext != NULL && snd->ext == NULL) return 1;
    else if(fst->ext == NULL && snd->ext != NULL) return -1;
    if(!strcmp(fst->ext,snd->ext) || (fst->ext==NULL && snd->ext==NULL)) return sort_by_name(a,b);
    return strcmp(fst->ext,snd->ext);
}

 /**
  * @brief funkcia triedi subory podla velkosti a pripony
  */
int sort_SE(const void *a, const void *b)
{
    file_t *fst = *(file_t**)a;
    file_t *snd = *(file_t**)b;
    if(fst->size < snd->size) return 1;
    else if(fst->size > snd->size) return -1;
    else return sort_by_extension(a,b);
}

  /**
  * @brief funkcia triedi subory podla pripony a velkosti
  */
int sort_ES(const void *a, const void *b)
{
    file_t *fst = *(file_t**)a;
    file_t *snd = *(file_t**)b;
    if(fst->ext != NULL && snd->ext == NULL) return 1;
    else if(fst->ext == NULL && snd->ext != NULL) return -1;
    if((fst->ext==NULL && snd->ext==NULL) || !strcmp(fst->ext,snd->ext)) return sort_by_size(a,b);
    return sort_by_extension(a,b);
}

  /**
  * @brief funkcia vybera sposob triedenia suborov
  * @param type, typ triedenia
  * @param arr_size, velkost pola
  * @param files, pole udajov o suboroch
  */
void choose_sort(SORT type, int arr_size, file_t **files)
{
    if(type==DEF) qsort(files,arr_size,sizeof(file_t*),sort_by_name);
    else if(type == F) qsort(files,arr_size,sizeof(file_t*),sort_by_path);
    else if(type == S) qsort(files,arr_size,sizeof(file_t*),sort_by_size);
    else if(type == E) qsort(files,arr_size,sizeof(file_t*),sort_by_extension);
    else if(type == SE) qsort(files,arr_size,sizeof(file_t*),sort_SE);
    else if(type == ES) qsort(files,arr_size,sizeof(file_t*),sort_ES);
}

  /**
   * @brief funkcia vypisuje zoradene subory
   * @param files, pole suborov
   * @param count, pocet suborov
   */
void print(file_t **files, int count)
{
    for(int i=0;i<count;i++)
    {
        printf("%s\n",files[i]->path);
        freeFile(files[i]);
    }
}
