/**
 *@author Jakub Micuda(433715)
 */
#include "BankRegister.h"
#include "Bank.h"
#include <iostream>

//Constructors

BankRegister::BankRegister():banksDB(){}


//Methods

uint BankRegister::registerBank(Bank* bank){
    uint id = 1;
    if(!banksDB.empty())
        id = banksDB.rbegin()->first + 1;
    banksDB.insert(std::make_pair(id,bank));
    return id;
}

void BankRegister::unregisterBank(uint bankNumber){
    banksMap::iterator it = banksDB.find(bankNumber);
    if(it!=banksDB.end()) {
        banksDB.erase(it);
    }
    else throw NonexistentBank();
}

Bank* BankRegister::bankByNumber(uint bankNumber) const{
    return byNumber<Bank,NonexistentBank>(bankNumber,banksDB); //template from BankRegister.h
}
