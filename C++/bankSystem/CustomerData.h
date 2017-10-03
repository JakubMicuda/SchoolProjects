/**
 *@author Jakub Micuda(433715)
 */
#ifndef CUSTOMERDATA_H
#define CUSTOMERDATA_H
#include <vector>
#include <string>

typedef unsigned int uint;

/**
 * @brief Storage for Customers Data in bank database
 */
class CustomerData
{
private:
    uint customerID;
    std::vector<uint> accounts;
    std::string name;
public:

    /**
     * @brief Parametric Constructor
     * @param number    customers ID
     * @param name      customers name
     */
    CustomerData(uint number, std::string const& name);

    /**
     * @brief Nonparametric Constructor
     */
    CustomerData();

    /**
     * @brief Getter for number
     * @return number
     */
    uint number() const;

    /**
     * @brief Getter accounts vector
     * @return vector
     */
    std::vector<uint> getAccounts() const;

    /**
     * @brief Getter for name
     * @return name
     */
    std::string const& getName() const;

    /**
     * @brief adds account to vector
     * @param number    ID of account
     */
    void addAccount(uint number);
};

#endif
