/**
 *@author Jakub Micuda(433715)
 */
#include "Interactive.h"

using namespace std;

void Interactive::createBank(std::string const & name){
    Bank* newBank = new Bank(name,&bankRegister);
    banks.push_back(newBank->number());
    cout << "Created bank: "<< name <<" | "<< banks.back() << endl;
}

void Interactive::createClient(uint bankNumber, std::string const & name){
    try{
        CustomerInterface* newClient = bankRegister.bankByNumber(bankNumber)->createCustomer(name);
        cout << "Created customer: "<< newClient->name() <<" | "<< newClient->number() << endl;
    }catch(NonexistentBank & ex){
        cout << ex << endl;
    }
}

void Interactive::createAccount(uint bankNumber, uint ownerNumber){
    try{
        AccountInterface* newAccount = bankRegister.bankByNumber(bankNumber)->customerByNumber(ownerNumber)->createAccount();
        cout << "Created account: "<< AccountID(newAccount->number(),bankNumber) << endl;
    }catch(NonexistentBank & ex){
        cout << ex << endl;
    }catch(NonexistentCustomer & ex){
        cout << ex << endl;
    }
}

void Interactive::deposit(AccountID target, uint amount){
    try{
        bankRegister.bankByNumber(target.bankNumber())->accountByNumber(target.accountNumber())->deposit(amount);
    }catch(NonexistentBank & ex){
        cout << ex << endl;
    }catch(NonexistentAccount & ex){
        cout << ex << endl;
    }
}

void Interactive::withdraw(AccountID target, uint amount){
    try{
        bankRegister.bankByNumber(target.bankNumber())->accountByNumber(target.accountNumber())->withdraw(amount);
    }catch(NonexistentBank & ex){
        cout << ex << endl;
    }catch(NonexistentAccount & ex){
        cout << ex << endl;
    }catch(InsufficientFunds & ex){
        cout << ex << endl;
    }
}

void Interactive::transfer(AccountID source, AccountID target, uint amount){
    try{
    bankRegister.bankByNumber(source.bankNumber())->accountByNumber(source.accountNumber())->transfer(amount,target);
    }catch(NonexistentBank & ex){
        cout << ex << endl;
    }catch(NonexistentAccount & ex){
        cout << ex << endl;
    }catch(InsufficientFunds & ex){
        cout << ex << endl;
    }
}

void Interactive::bankInfo(uint bankNumber){
    try{
        cout << *bankRegister.bankByNumber(bankNumber) << endl;
    }catch(NonexistentBank & ex){
        cout << ex << endl;
    }
}

void Interactive::customerInfo(uint bankNumber, uint customerID){
    try{
        cout << *bankRegister.bankByNumber(bankNumber)->customerByNumber(customerID) << endl;
    }catch(NonexistentBank & ex){
        cout << ex << endl;
    }catch(NonexistentCustomer & ex){
        cout << ex << endl;
    }
}

void Interactive::accountInfo(AccountID target){
    try{
        cout << *bankRegister.bankByNumber(target.bankNumber())->accountByNumber(target.accountNumber()) << endl;
    }catch(NonexistentBank & ex){
        cout << ex << endl;
    }catch(NonexistentAccount & ex){
        cout << ex << endl;
    }
}

void Interactive::clearRegister(){
    Bank* toBeDeleted;
    while(!banks.empty()){
        toBeDeleted = bankRegister.bankByNumber(banks.back());
        bankRegister.unregisterBank(banks.back());
        delete toBeDeleted;
        banks.pop_back();
    }
}

/****************************OPERATORS*********************************/

//Input

std::istream & operator>>(std::istream & in, AccountID & accountID){
    string line;
    char slash;
    in >> line;
    istringstream input(line);
    uint number, bankNumber;
    input >> number >> slash >> bankNumber;
    accountID = AccountID(number,bankNumber);
    return in;
}

//Output

ostream & operator<<(ostream & out, const AccountID & accountID){
    out << accountID.accountNumber()<<"/"<<accountID.bankNumber();
    return out;
}

ostream & operator<<(ostream & out, BankException & ex){
    out << "Exception: " << ex.what();
    return out;
}

ostream & operator<<(ostream & out, Bank & bank){
    out << "Name: " << bank.name() << endl;
    out << "Customers: ";
    if(bank.customers().empty()) out << 0 << endl;
    else out << bank.customers().rbegin()->first << endl;
    out << "Accounts: ";
    if(bank.accounts().empty()) out << 0;
    else out << bank.accounts().rbegin()->first;
    return out;
}

ostream & operator<<(ostream & out, CustomerInterface & customer){
    out << "Name: " << customer.name() << endl;
    out << "Accounts: ";
    if(customer.accounts().empty()) {
        return out;
    }
    for(AccountInterface* it : customer.accounts()) {
        out << it->number();
        if(it != customer.accounts().back())
        {
            out << ", ";
        }
    }
    return out;
}

ostream & operator<<(ostream & out, AccountInterface & account){
    out << "Balance: " << account.balance();
    return out;
}
