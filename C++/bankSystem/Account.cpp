/**
 *@author Jakub Micuda(433715)
 */
#include "Account.h"
#include "Bank.h"
#include "Database.h"

/********************ACCOUNTID IMPLEMENTATION*****************************/

//Constructors

AccountID::AccountID(uint accountNumber, uint bankNumber):
    accountID(std::make_pair(accountNumber,bankNumber)){}

AccountID::AccountID():accountID(){}

//Getters

uint AccountID::accountNumber() const{
    return accountID.first;
}

uint AccountID::bankNumber() const{
    return accountID.second;
}

//Operators

AccountID & AccountID::operator=(const AccountID & data){
    if(this != &data){
        accountID = data.accountID;
    }
    return *this;
}

/********************BASIC ACCOUNT IMPLEMENTATION************************/

//Constructors

BasicAccount::BasicAccount(uint number, Bank* bank, Database* db):
    accountID(number),
    myDatabase(db),
    myBank(bank){}

//Methods

void BasicAccount::deposit(uint amount){
    AccountData myData = myDatabase->loadAccount(accountID);
    myData.setBalance(myData.balance()+amount);
    myDatabase->save(myData);
}

void BasicAccount::withdraw(uint amount){
    AccountData myData = myDatabase->loadAccount(accountID);
    if(amount > myData.balance()) throw InsufficientFunds();
    else{
        myData.setBalance(myData.balance()-amount);
        myDatabase->save(myData);
    }
}

void BasicAccount::transfer(uint amount, AccountID target){
    myBank->transfer(amount,accountID,target);
}

//Getters

uint BasicAccount::balance() const{
    return myDatabase->loadAccount(accountID).balance();
}

uint BasicAccount::number() const{
    return accountID;
}

//Destructors

BasicAccount::~BasicAccount(){}
