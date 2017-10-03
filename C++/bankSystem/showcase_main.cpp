#include <iostream>

#include "Bank.h"
#include "Account.h"
#include "Customer.h"
#include "BankRegister.h"

using namespace std;

int main()
{
    BankRegister mainRegister;
    Bank bank1("Bank1", &mainRegister);
    Bank bank2("Bank2", &mainRegister);

    bank1.createCustomer("Customer11")->createAccount()->deposit(1000);
    bank1.createCustomer("Customer12")->createAccount()->deposit(1000);

    bank2.createCustomer("Customer21")->createAccount()->deposit(1000);
    bank2.createCustomer("Customer22")->createAccount()->deposit(1000);

    bank1.transfer(500, 1, AccountID(2, 1));
    bank2.transfer(500, 1, AccountID(2, 2));

    cout << "Balance on account 1 in bank 1: " << bank1.accountByNumber(1)->balance() << '\n';
    cout << "Balance on account 2 in bank 1: " << bank1.accountByNumber(2)->balance() << '\n';
    cout << "Balance on account 1 in bank 2: " << bank2.accountByNumber(1)->balance() << '\n';
    cout << "Balance on account 2 in bank 2: " << bank2.accountByNumber(2)->balance() << '\n' << endl;

    bank1.transfer(500, 2, AccountID(2, 2));

    cout << "Balance on account 1 in bank 1: " << bank1.accountByNumber(1)->balance() << '\n';
    cout << "Balance on account 2 in bank 1: " << bank1.accountByNumber(2)->balance() << '\n';
    cout << "Balance on account 1 in bank 2: " << bank2.accountByNumber(1)->balance() << '\n';
    cout << "Balance on account 2 in bank 2: " << bank2.accountByNumber(2)->balance() << endl;


    return 0;
}

