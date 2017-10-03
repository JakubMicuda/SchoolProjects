/**
 *@author Jakub Micuda(433715)
 */
#ifndef ACCOUNTDATA_H
#define ACCOUNTDATA_H

typedef unsigned int uint;


/**
 * @brief Storage for Account Data in bank database
 */
class AccountData
{
private:
    uint customerID;
    uint accountID;
    uint myBalance;
public:

    /**
     * @brief Parametric Constructor
     * @param number        ID of account in database
     * @param ownerNumber   Owners ID in database
     */
    AccountData(uint number, uint ownerNumber);

    /**
     * @brief Nonparametric Constructor
     */
    AccountData();

    /**
     * @brief Getter for accountID
     * @return accountID
     */
    uint number() const;

    /**
     * @brief Getter for customerID
     * @return customerID
     */
    uint ownerNumber() const;

    /**
     * @brief Getter for myBalance
     * @return myBalance
     */
    uint balance() const;

    /**
     * @brief Setter for myBalance
     * @param balance
     */
    void setBalance(uint balance);
};

#endif
