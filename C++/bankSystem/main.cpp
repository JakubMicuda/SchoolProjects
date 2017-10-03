/**
 *@author Jakub Micuda(433715)
 */
#include <iostream>
#include <sstream>
#include "Interactive.h"

using namespace std;

Command toCommand(string const & line);

int main(){
    Interactive interface;
    string input;

    //temporary inputs
    uint number, bankNumber, amount;
    AccountID target;
    AccountID source;
    string name;
    size_t firstSpace;

    bool quit = false;

    //while quit command is not given
    while(!quit){
        getline(cin,input);

        firstSpace = input.find_first_of(" \n");
        istringstream parameters(input.substr(firstSpace + 1) );

        //Switches input by Command to determine which method to use and how to parse input
        switch(toCommand(input.substr(0,firstSpace)))
        {
            case CREATEBANK:
                getline(parameters,name);
                interface.createBank(name);
            break;

            case CREATECLIENT:
                parameters >> number;
                getline(parameters,name);
                interface.createClient(number,name.substr(1));
            break;

            case CREATEACCOUNT:
                parameters >> bankNumber >> number;
                interface.createAccount(bankNumber,number);
            break;

            case DEPOSIT:
                parameters >> amount >> target;
                interface.deposit(target,amount);
            break;

            case WITHDRAW:
                parameters >> amount >> target;
                interface.withdraw(target,amount);
            break;

            case TRANSFER:
                parameters >> amount >> source;
                parameters >> target;
                interface.transfer(source,target,amount);
            break;

            case BANKINFO:
                parameters >> bankNumber;
                interface.bankInfo(bankNumber);
            break;

            case CUSTOMERINFO:
                parameters >> bankNumber >> number;
                interface.customerInfo(bankNumber,number);
            break;

            case ACCOUNTINFO:
                parameters >> target;
                interface.accountInfo(target);
            break;

            case QUIT:
                quit = true;
            break;

            default:
            break;
        }   //end of switch
   }   //end of while
   interface.clearRegister(); //clears register and deallocates all dynamic memory
 return 0;
}


/**
 * @brief function to check which command is given string
 * @param line  string input
 * @return      Command
 */
Command toCommand(string const & line){
    if(!line.compare("create_bank")) return CREATEBANK;
    if(!line.compare("create_customer")) return CREATECLIENT;
    if(!line.compare("create_account")) return CREATEACCOUNT;
    if(!line.compare("deposit")) return DEPOSIT;
    if(!line.compare("withdraw")) return WITHDRAW;
    if(!line.compare("transfer")) return TRANSFER;
    if(!line.compare("bank_info")) return BANKINFO;
    if(!line.compare("customer_info")) return CUSTOMERINFO;
    if(!line.compare("account_info")) return ACCOUNTINFO;
    if(!line.compare("quit")) return QUIT;
    return NONE;
}

