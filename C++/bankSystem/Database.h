/**
 *@author Jakub Micuda(433715)
 */
#ifndef DATABASE_H
#define DATABASE_H

#include "CustomerData.h"
#include "AccountData.h"
#include "BankRegister.h"


#include <string>
#include <map>
#include <utility>


typedef unsigned int uint;
typedef std::map<uint, CustomerData> CustomerDatabase;
typedef std::map<uint, AccountData> AccountDatabase;


/**
 * @brief Interface for manipulating banks database
 */
class Database
{
private:
    CustomerDatabase myCustomers;
    AccountDatabase myAccounts;
public:

    /**
     * @brief creates customer in database
     * @param name  customers name
     * @return      customers data
     */
    CustomerData createCustomer(std::string const& name);

    /**
     * @brief creates account in database
     * @param ownerNumber   customers ID
     * @return              accounts data
     */
    AccountData createAccount(uint ownerNumber);

    /**
     * @brief loads customers data from database
     * @param number    customers ID
     * @return          customers data
     */
    CustomerData loadCustomer(uint number) const;

    /**
     * @brief loads accounts data from database
     * @param number    accounts ID
     * @return          accounts data
     */
    AccountData loadAccount(uint number) const;

    /**
     * @brief saves customers data to database
     */
    void save(CustomerData);

    /**
     * @brief saves accounts data to database
     */
    void save(AccountData);

    /**
     * @brief Getter for customers map
     * @return customers map
     */
    CustomerDatabase const& customers() const;

    /**
     * @brief Getter for accounts map
     * @return accounts map
     */
    AccountDatabase const& accounts() const;
};

#endif
