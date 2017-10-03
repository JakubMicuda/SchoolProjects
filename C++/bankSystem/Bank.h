/**
 *@author Jakub Micuda(433715)
 */
#ifndef BANK_H
#define BANK_H
#include "Database.h"
#include "AccountID.h"

#include <string>
#include <vector>
#include <utility>
#include <algorithm>
#include <map>

typedef unsigned int uint;

//header doesn't need to know implementation of these classes
class BankRegister;
class CustomerInterface;
class AccountInterface;

typedef std::map<uint, CustomerInterface*> customersMap;
typedef std::map<uint, AccountInterface*> accountsMap;

/**
 * @brief Class to manipulate customers/accounts data and store them in Database
 */
class Bank
{
private:
    customersMap customersDB;
    accountsMap accountsDB;
    Database database;
    std::string bankName;
    BankRegister* myRegister;
    uint bankID;
public:

    /**
     * @brief Parametric Constructor
     * @param name  banks name
     * @param centralRegister pointer to BankRegister
     */
    Bank(std::string const& name, BankRegister* centralRegister);

    /**
     * @brief NonParametric Constructor
     */
    Bank();

    /**
     * @brief creates item in database and interface for customer
     * @param name  name of customer
     * @return Customers interface to manipulate with customer
     */
    CustomerInterface* createCustomer(std::string const& name);

    /**
     * @brief creates item in database and interface for account
     * @param ownerNumber   ID of owner
     * @return Accounts interface
     */
    AccountInterface* createAccount(uint ownerNumber);

    /**
     * @brief searches for customer in database by his number
     * @param customerNumber    customers ID in database
     * @return Customers interface
     */
    CustomerInterface* customerByNumber(uint customerNumber);

    /**
     * @brief searches for account in database by its number
     * @param accountNumber     accounts ID in database
     * @return Accounts interface
     */
    AccountInterface* accountByNumber(uint accountNumber);

    /**
     * @brief Getter for map of customers
     * @return customers
     */
    customersMap const& customers();

    /**
     * @brief Getter for map of accounts
     * @return accounts
     */
    accountsMap const& accounts();

    /**
     * @brief Method for transfering money from source to target
     * @param amount    amount of money to be transfered
     * @param source    source ID in this bank
     * @param target    AccountID of target
     */
    void transfer(uint amount, uint source, AccountID target);

    /**
     * @brief Getter for banks name
     * @return bankName
     */
    std::string const& name() const;

    /**
     * @brief Getter for banks number
     * @return bankID
     */
    uint number() const;

    /**
     *  Destructor
     */
    ~Bank();
};

#endif 
