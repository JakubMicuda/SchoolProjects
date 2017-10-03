/**
 * @Author Jakub Micuda(433715)
 */
#include "fill.h"
#include "utility.h"
#include <iostream>
#include <fstream>

/**
 * @brief loadError sets m_correct to false, and returns given error message
 * @param message   Error message to be returned
 * @param m_correct sets this parameter to false
 * @return          given error message
 */
Error loadError(Error message,bool &m_correct)
{
    m_correct = false;
    return message;
}


/*****************************************************************************/

Error EdgeMap::load(const string &path)
{
    ifstream input;
    input.open(path,std::ios::in);
    if(!input) {m_correct = false; return Error::E_CANTOPEN;}

    //allocating memory of vector
    input.seekg(0,input.end);
    m_edgeMap.reserve(input.tellg());
    input.seekg(0,input.beg);

    bool seedFound = false;
    bool newLineReached = false;
    Point currPos(0,0);

    char c;
    input.get(c);
    while(!input.eof())
    {

        switch(c) {
            //if there was already found seed, method returns Error message
            case PIXEL_SEED:
                if(seedFound) return loadError(Error::E_MORESEEDS,m_correct);
                m_seed = currPos;
                m_edgeMap.push_back(PIXEL_SEED);
                seedFound = true;
                currPos.setX(currPos.getX()+1);
            break;

            case PIXEL_BLANK:
                m_edgeMap.push_back(PIXEL_BLANK);
                currPos.setX(currPos.getX()+1);
            break;

            case PIXEL_BORDER:
                m_edgeMap.push_back(PIXEL_BORDER);
                currPos.setX(currPos.getX()+1);
            break;

            //after first '\n', mapsize of X is set, and when next '\n' is found,
            //it checks if position X is same as mapsize, if not, method returns Error message
            case '\n':
                if(!input.eof()) {
                    if(newLineReached) {
                        if(m_mapSize.getX()!=currPos.getX()) return loadError(Error::E_SIZE,m_correct);
                    }else {
                        newLineReached = true;
                        m_mapSize.setX(currPos.getX());
                    }
                    currPos.setY(currPos.getY()+1);
                    currPos.setX(0);
                 }
            break;

            default:
                return loadError(Error::E_INVALIDCHAR,m_correct);
        }
        input.get(c);
    }
    if(!seedFound) return loadError(Error::E_NOSEED,m_correct);
    m_mapSize.setY(currPos.getY());
    m_correct = true;
    input.close();
    return Error::E_OK;
}

/*****************************************************************************/

//Im sorry for repetitive code, but i have no access to fill.h,
//so i can't add more private methods and then use getIndex()
Error EdgeMap::fill(ushort &area)
{
    if(!m_correct) return Error::E_NOTLOADED;
    DynamicData myStack;
    myStack.push(m_seed);
    Point seed;
    Point fill;
    bool seedFound = false;

    //loops until stack is empty
    while(!myStack.empty()) {
        //popping seed from stack
        myStack.pop(seed);
        fill = seed;

        //checking if seed is on the edge of map, if yes, method returns Error message
        //else, seed is replaced with color and count is increased
        if(seed.getY()+1 == m_mapSize.getY() || seed.getY() == 0) return Error::E_OUTOFMAP;
        if(m_edgeMap[getIndex(seed.getX(),seed.getY())]!=PIXEL_FILLED){
            m_edgeMap[getIndex(seed.getX(),seed.getY())] = PIXEL_FILLED;
            area++;
        }

        //setting fill to left pixel from seed, if that pixel is on left edge, returns Error message
        //it prevents accessing points outside array
        fill.setX(seed.getX()-1);
        if(seed.getX() == 0) return Error::E_OUTOFMAP;

        //loops and fills left pixels until fill is not border, or left edge of map
        while(m_edgeMap[getIndex(fill.getX(),fill.getY())]==PIXEL_BLANK)
        {
                m_edgeMap[getIndex(fill.getX(),fill.getY())] = PIXEL_FILLED;
                area++;
            if(fill.getX() == 0) return Error::E_OUTOFMAP;
            fill.setX(fill.getX()-1);
        }


        //setting fill to right pixel from seed, if that pixel is on right edge, returns Error message
        //it prevents accessing points outside array
        fill.setX(seed.getX()+1);
        if(fill.getX() >= m_mapSize.getX()) return Error::E_OUTOFMAP;

        //loops and fills right pixels until fill is not border, or right edge of map
        while(m_edgeMap[getIndex(fill.getX(),fill.getY())]==PIXEL_BLANK)
        {
                m_edgeMap[getIndex(fill.getX(),fill.getY())] = PIXEL_FILLED;
                area++;
            fill.setX(fill.getX()+1);
            if(fill.getX() >= m_mapSize.getX()) return Error::E_OUTOFMAP;
        }

        //checks for seeds on line above
        //seed is set on first left occurence of filled neighbour and pushed on stack
        if(seed.getY()-1 >= 0) {
            for(int i=0;i<m_mapSize.getX();i++)
            {
                //if seedFound is true algorithm checks for border, if it finds one,
                //then it checks for another seed
                if(!seedFound) {
                    if(m_edgeMap[getIndex(i,seed.getY())]==PIXEL_FILLED && m_edgeMap[getIndex(i,seed.getY()-1)]==PIXEL_BLANK) {
                        myStack.push(Point(i,seed.getY()-1));
                        seedFound = true;
                    }
                }else if(m_edgeMap[getIndex(i,seed.getY()-1)]==PIXEL_BORDER) seedFound = false;
            }
        }

        //checks for seeds on line under
        if(seed.getY()+1 < m_mapSize.getY()) {
            for(int i=0;i<m_mapSize.getX();i++)
            {
                if(!seedFound) {
                    if(m_edgeMap[getIndex(i,seed.getY())]==PIXEL_FILLED && m_edgeMap[getIndex(i,seed.getY()+1)]==PIXEL_BLANK) {
                        myStack.push(Point(i,seed.getY()+1));
                        seedFound = true;
                    }
                }else if(m_edgeMap[getIndex(i,seed.getY()+1)]==PIXEL_BORDER) seedFound = false;
            }
        }
    }
    return Error::E_OK;
}

/*****************************************************************************/

Error EdgeMap::print() const
{
    if(!m_correct) return Error::E_NOTLOADED;
    for(int i=0;i<m_mapSize.getY();i++)
    {
        for(int j=0;j<m_mapSize.getX();j++)
        {
            cout << m_edgeMap[getIndex(j,i)];
        }
        cout << endl;
    }
    return Error::E_OK;
}

/*****************************************************************************/

ushort EdgeMap::getIndex(ushort x, ushort y) const
{
    return m_mapSize.getX()*y + x;
}

/*****************************************************************************/
