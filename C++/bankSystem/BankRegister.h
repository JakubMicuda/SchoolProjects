/**
 *@author Jakub Micuda(433715)
 */
#ifndef BANK_REGISTER_H
#define BANK_REGISTER_H

#include "BankExceptions.h"

#include <map>
#include <utility>

class Bank;

typedef unsigned int uint;
typedef std::map<uint,Bank*> banksMap;

/**
 *  Template function for searching item in <uint, item> map by its number
 */
template<class T,class U>
T* byNumber(uint number,const std::map<uint,T*> & dataDB){
    typename std::map<uint,T*>::const_iterator it = dataDB.find(number);
    if(it!=dataDB.end()) {
        return it->second;
    }else throw U();
}

/**
 * @brief Class to manipulate with banks
 */
class BankRegister
{
private:
    banksMap banksDB;
public:

    /**
     * @brief Nonparametric Constructor
     */
    BankRegister();

    /**
     * @brief registers Bank in this register
     * @param bank  pointer to Bank
     * @return ID of Bank in register
     */
    uint registerBank(Bank* bank);

    /**
     * @brief removes Bank from this register
     * @param bankNumber    ID of Bank
     */
    void unregisterBank(uint bankNumber);

    /**
     * @brief searches for Bank in register by its number
     * @param bankNumber
     * @return pointer to Bank
     */
    Bank* bankByNumber(uint bankNumber) const;
};

#endif
 
