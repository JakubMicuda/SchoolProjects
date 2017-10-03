/**
 *@author Jakub Micuda(433715)
 */
#include "Database.h"

using namespace std;

/*********************************TEMPLATES*********************************/

template<class T,typename U>
T create(U data, std::map<uint, T> & database){
    uint id = 1;
    if(!database.empty()) id = database.rbegin()->first + 1;
    database.insert(make_pair(id,T(id,data)));
    return database.rbegin()->second;
}

template<class T>
T load(uint number,const std::map<uint,T> & database,BankException ex){
    typename std::map<uint,T>::const_iterator it = database.find(number);
    if(it!=database.end()) return it->second;
    else throw ex;
}

template<class T>
void saveData(T data,std::map<uint,T> & database){
    typename std::map<uint,T>::iterator it = database.find(data.number());
    it->second = data;
}



/***********************ACCOUNT DATA IMPLEMENTATION**************************/

//Constructors

AccountData::AccountData(uint number, uint ownerNumber):
    customerID(ownerNumber),
    accountID(number),
    myBalance(0){}


//Getters

uint AccountData::number() const{
    return accountID;
}

uint AccountData::ownerNumber() const{
    return customerID;
}

uint AccountData::balance() const{
    return myBalance;
}


//Setters

void AccountData::setBalance(uint balance){
    myBalance = balance;
}

/***********************CUSTOMER DATA IMPLEMENTATION******************************/

//Constructors

CustomerData::CustomerData(uint number, string const& name):
    customerID(number),
    accounts(),
    name(name){}


//Methods

void CustomerData::addAccount(uint number){
    accounts.push_back(number);
}


//Getters

uint CustomerData::number() const{
    return customerID;
}
vector<uint> CustomerData::getAccounts() const{
    return accounts;
}
string const& CustomerData::getName() const{
    return name;
}

/***************************DATABASE IMPLEMENTATION*******************************/

//Methods

CustomerData Database::createCustomer(string const& name){
    return create<CustomerData,string>(name,myCustomers);
}

AccountData Database::createAccount(uint ownerNumber){
    return create<AccountData,uint>(ownerNumber,myAccounts);
}

CustomerData Database::loadCustomer(uint number) const{
    return load<CustomerData>(number,myCustomers,NonexistentCustomer());
}

AccountData Database::loadAccount(uint number) const{
    return load<AccountData>(number,myAccounts,NonexistentAccount());
}

void Database::save(CustomerData data){
    saveData<CustomerData>(data,myCustomers);
}

void Database::save(AccountData data){
    saveData<AccountData>(data,myAccounts);
}


//Getters

CustomerDatabase const& Database::customers() const{
    return myCustomers;
}
AccountDatabase const& Database::accounts() const{
    return myAccounts;
}
