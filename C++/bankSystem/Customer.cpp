/**
 *@author Jakub Micuda(433715)
 */
#include "Customer.h"
#include "Bank.h"
#include "Database.h"

using namespace std;


//Constructors

BasicCustomer::BasicCustomer(uint number, string const & name,Bank* bank):
    myAccountInterfaces(),
    customerID(number),
    myName(name),
    myBank(bank){}


//Methods

AccountInterface* BasicCustomer::createAccount(){
    AccountInterface* newAccount= myBank->createAccount(customerID);
    myAccountInterfaces.push_back(newAccount);
    return newAccount;
}


//Getters

std::vector<AccountInterface*> const& BasicCustomer::accounts(){
    return myAccountInterfaces;
}

std::string const& BasicCustomer::name() const{
    return myName;
}

uint BasicCustomer::number() const{
    return customerID;
}


//Destructors

BasicCustomer::~BasicCustomer(){}
