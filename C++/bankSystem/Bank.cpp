/**
 *@author Jakub Micuda(433715)
 */
#include "Bank.h"
#include "Customer.h"
#include "Account.h"
#include "BankRegister.h"
#include <iostream>

using namespace std;

//Constructors

Bank::Bank(const std::string &name, BankRegister *centralRegister):
customersDB(),
accountsDB(),
database(),
bankName(name),
myRegister(centralRegister){
    bankID = centralRegister->registerBank(this);
}


//Methods

CustomerInterface* Bank::createCustomer(string const& name){
    CustomerData newCustomer = database.createCustomer(name);
    CustomerInterface* newCustomerInterface = new BasicCustomer(newCustomer.number(),newCustomer.getName(),this);
    customersDB.insert(std::make_pair(newCustomer.number(),newCustomerInterface));
    return newCustomerInterface;
}

AccountInterface* Bank::createAccount(uint ownerNumber){
    AccountData newAccount = database.createAccount(ownerNumber);
    AccountInterface* newAccountIterface = new BasicAccount(newAccount.number(),this,&database);
    accountsDB.insert(std::make_pair(newAccount.number(),newAccountIterface));
    return newAccountIterface;
}

CustomerInterface* Bank::customerByNumber(uint customerNumber){
    return byNumber<CustomerInterface,NonexistentCustomer>(customerNumber,customersDB);
}

AccountInterface* Bank::accountByNumber(uint accountNumber){
    return byNumber<AccountInterface,NonexistentAccount>(accountNumber,accountsDB);
}

void Bank::transfer(uint amount, uint source, AccountID target){
      if(AccountID(source,bankID) != target){
        AccountInterface* targetAccount = myRegister->bankByNumber(target.bankNumber())->accountByNumber(target.accountNumber());
        accountByNumber(source)->withdraw(amount);
        targetAccount->deposit(amount);
      }
}


//Getters

customersMap const& Bank::customers(){
    return customersDB;
}

accountsMap const& Bank::accounts(){
    return accountsDB;
}

std::string const& Bank::name() const{
    return bankName;
}

uint Bank::number() const{
    return bankID;
}


//Destructors

Bank::~Bank(){
    while(!customersDB.empty()){
            delete customersDB.rbegin()->second;
            customersDB.erase(customersDB.rbegin()->first);
    }

    while(!accountsDB.empty()){
            delete accountsDB.rbegin()->second;
            accountsDB.erase(accountsDB.rbegin()->first);
    }
}
