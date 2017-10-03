#ifndef CUSTOMER_H
#define CUSTOMER_H

/**
 *@author Jakub Micuda(433715)
 */
#include "CustomerInterface.h"
#include <algorithm>

//this header doesn't need to know implementation of these classes
class Bank;
class Database;

typedef unsigned int uint;

/**
 * @brief Interface for manipulating customers data of bank
 */
class BasicCustomer : public CustomerInterface
{
private:
    std::vector<AccountInterface*> myAccountInterfaces;
    uint customerID;
    std::string myName;
    Bank* myBank;
public:
    /**
     * @brief Parametric Constructor
     * @param number    customers ID
     * @param name      customers name
     * @param bank      customers bank name
     */
    BasicCustomer(uint number, std::string const & name, Bank* bank);

    /**
     * @brief creates account attached to this customer
     * @return  accounts interface
     */
    AccountInterface* createAccount();

    /**
     * @brief Getter for vector of AccountInterfaces
     * @return vector
     */
    std::vector<AccountInterface*> const& accounts();

    /**
     * @brief Getter for customers name
     * @return
     */
    std::string const& name() const;

    /**
     * @brief Getter for customers ID
     * @return number
     */
    uint number() const;

    /**
     * Destructor
     */
    ~BasicCustomer();
};


#endif 
