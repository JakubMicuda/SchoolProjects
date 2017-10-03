/**
 * @Author Jakub Micuda(433715)
 */
/*#define CATCH_CONFIG_MAIN
#include "catch.hpp"

#include "BarCodeEAN13.h"
#include "BarCodeDataMatrix.h"

using namespace std;

/**
 * BarCodeDataMatrix Tests Note:
 *
 * To use these tests you need to add public function bool** data() into CBarCodeDataMatrix class.
 * It should return m_data attribute. !DONT FORGET TO DELETE IT AFTER TESTING!
 * These tests work only if you add data to m_data row by row.
 * Example: input="11 00"
 *          m_data[0][0] = true
 *          m_data[0][1] = true
 *          m_data[1][0] = false
 *          m_data[1][1] = false
 */

/*TEST_CASE("BarCodeEAN13: constructor (int *)") {

    SECTION("null input test") {
        int *eancode = nullptr;
        CBarCodeEAN13 test(eancode);
        for(unsigned int i=0; i<12; i++) {
            REQUIRE(test.digits()[i]==0);
        }
    }

    SECTION("invalid EAN13 numbers test") {
        int eancode[] = {1,0,-15,50,60,3,2,7,4,7,8,9};
        CBarCodeEAN13 test(eancode);
        for(unsigned int i=0; i<12; i++) {
            REQUIRE(test.digits()[i]==eancode[i]);
        }
        REQUIRE(test.checkSum() == 2);
    }

    SECTION("valid EAN13 numbers test") {
        int eancode[] = {1,2,3,4,5,6,7,8,9,1,2,1};
        CBarCodeEAN13 test(eancode);
        for(unsigned int i=0; i<12; i++)
            REQUIRE(test.digits()[i]==eancode[i]);

        REQUIRE(test.checkSum() == 7);
    }

    SECTION("valid long input test") {
        int eancode[] = {1,2,3,4,5,6,7,8,9,1,2,1,5,7,8,9};
        CBarCodeEAN13 test(eancode);
        for(unsigned int i=0; i<12; i++)
            REQUIRE(test.digits()[i]==eancode[i]);

        REQUIRE(test.checkSum() == 7);
    }
}

TEST_CASE("BarCodeEAN13: zero()") {

    int eancode[] = {1,0,-15,50,60,3,2,7,4,7,8,9};
    CBarCodeEAN13 test(eancode);
    test.zero();
    for(unsigned int i=0; i<12; i++) {
        REQUIRE(test.digits()[i] == 0);
    }
    REQUIRE(test.checkSum()==0);
}

TEST_CASE("BarCodeEAN13: isValid()") {
    SECTION("invalid numbers test"){
        int eancode[] = {1,0,-15,50,60,3,2,7,4,7,8,9};
        CBarCodeEAN13 test(eancode);

        REQUIRE_FALSE(test.isValid());
    }

    SECTION("valid numbers test") {
        int eancode[] = {1,0,5,6,7,2,6,3,9,4,7,7};
        CBarCodeEAN13 test(eancode);
        REQUIRE(test.isValid());
    }

}

TEST_CASE("BarCodeEAN13: bool parseInput(const std::string& data)") {
    SECTION("invalid characters on input test") {
        int eancode[] = {1,0,3,2,60,3,2,7,4,7,8,9};
        CBarCodeEAN13 test(eancode);
        REQUIRE_FALSE(test.parseInput("yzs5678421!s"));
        test.parseInput("1345usasd212");
        REQUIRE(test.digits()[0]==1);
        REQUIRE(test.digits()[1]==3);
        REQUIRE(test.digits()[2]==4);
        REQUIRE(test.digits()[3]==5);
    }

    SECTION("white spaces test") {
        int eancode[] = {1,2,3,4,5,6,7,8,9,1,2,3};
        CBarCodeEAN13 test(eancode);
        test.parseInput("1234 56 789 123");
        for(unsigned int i=0; i<12; i++) {
            REQUIRE(test.digits()[i]== eancode[i]);
        }
    }

    SECTION("shorten input then 12 test") {
        int eancode[] = {1,2,3,4,5,6,7,8,9,1,2,3};
        CBarCodeEAN13 test(eancode);
        REQUIRE_FALSE(test.parseInput("1234"));
    }
}

TEST_CASE("BarCodeEAN13: CBarCodeEAN13(const std::string& eanCode)") {
    CBarCodeEAN13 test("123456789123");
    int eancode[] = {1,2,3,4,5,6,7,8,9,1,2,3};
    for(unsigned int i=0; i<12; i++)
        REQUIRE(test.digits()[i]==eancode[i]);

    REQUIRE(test.checkSum() == 1);
}

TEST_CASE("BarCodeDataMatrix: bool parseInput(const std::string& data)") {

    SECTION("test invalid characters on input") {
        CBarCodeDataMatrix *test = new CBarCodeDataMatrix("11 00", 2);
        REQUIRE_FALSE(test->parseInput("za !1"));
        test->parseInput("11 zz");
        REQUIRE(test->data()[0][0]==true);
        REQUIRE(test->data()[0][1]==true);
        delete test;

        test = new CBarCodeDataMatrix("",2);
        REQUIRE(test->data()[0][0]==false);
        REQUIRE(test->data()[0][1]==false);
        REQUIRE(test->data()[1][0]==false);
        REQUIRE(test->data()[1][1]==false);
    }

    SECTION("test white spaces ignoring") {
        CBarCodeDataMatrix *test = new CBarCodeDataMatrix("111 000 111", 3);
        test->parseInput("1  1 0   101 0   10");
        REQUIRE(test->data()[0][0]==true);
        REQUIRE(test->data()[0][1]==true);
        REQUIRE(test->data()[0][2]==false);
        REQUIRE(test->data()[1][0]==true);
        REQUIRE(test->data()[1][1]==false);
        REQUIRE(test->data()[1][2]==true);
        REQUIRE(test->data()[2][0]==false);
        REQUIRE(test->data()[2][1]==true);
        REQUIRE(test->data()[2][2]==false);
        delete test;
    }

    SECTION("test valid input") {
       CBarCodeDataMatrix *test = new CBarCodeDataMatrix("111 000 111", 3);
       REQUIRE(test->parseInput("111 000 111"));
       REQUIRE(test->data()[0][0]==true);
       REQUIRE(test->data()[0][1]==true);
       REQUIRE(test->data()[0][2]==true);
       REQUIRE(test->data()[1][0]==false);
       REQUIRE(test->data()[1][1]==false);
       REQUIRE(test->data()[1][2]==false);
       REQUIRE(test->data()[2][0]==true);
       REQUIRE(test->data()[2][1]==true);
       REQUIRE(test->data()[2][2]==true);
       delete test;
    }

    SECTION("test long valid input") {
       CBarCodeDataMatrix *test = new CBarCodeDataMatrix("11 00", 2);
       REQUIRE(test->parseInput("111 000 111"));
       REQUIRE(test->data()[0][0]==true);
       REQUIRE(test->data()[0][1]==true);
       REQUIRE(test->data()[1][0]==true);
       REQUIRE(test->data()[1][1]==false);
       delete test;
    }

    SECTION("test short valid input") {
        CBarCodeDataMatrix *test = new CBarCodeDataMatrix("111 000 111", 3);
        REQUIRE_FALSE(test->parseInput("11 00"));
        REQUIRE(test->data()[1][1]==false);
        REQUIRE(test->data()[1][2]==false);
        REQUIRE(test->data()[2][0]==false);
        REQUIRE(test->data()[2][1]==false);
        REQUIRE(test->data()[2][2]==false);
        delete test;
    }

    SECTION("test bad allocation") {
        CBarCodeDataMatrix *test = new CBarCodeDataMatrix("111 000 111", 0);
        REQUIRE_FALSE(test->parseInput("111 000 111"));
        delete test;

        test = new CBarCodeDataMatrix("11 00",500000000);
        REQUIRE_FALSE(test->parseInput("11 00"));
        delete test;
    }
}

TEST_CASE("BarCodeDataMatrix: CBarCodeDataMatrix(const std::string& data, std::size_t dataMatrixSize)") {
    SECTION("dataMatrixSize = 0 || dataMatrixSize < 0 || null inputs") {
        CBarCodeDataMatrix *test = new CBarCodeDataMatrix("111 000 111", 0);
        REQUIRE(test->data()==nullptr);
        test->zero();
        test->print();
        delete test;

        test = new CBarCodeDataMatrix("111 000 111", -1);
        REQUIRE(test->data()==nullptr);
        test->zero();
        test->print();
        delete test;

        test = new CBarCodeDataMatrix("11 00",500000000);
        REQUIRE(test->data()==nullptr);
        test->zero();
        test->print();
        delete test;

       /* test = new CBarCodeDataMatrix(nullptr,2);
        test->zero();
        test->print();*/
/*    }
}

TEST_CASE("BarCodeDataMatrix: zero()") {
    CBarCodeDataMatrix *test = new CBarCodeDataMatrix("11 11", 2);
    test->zero();
    REQUIRE(test->data()[0][0]==false);
    REQUIRE(test->data()[0][1]==false);
    REQUIRE(test->data()[1][0]==false);
    REQUIRE(test->data()[1][1]==false);
    delete test;
}

TEST_CASE("BarCodeDataMatrix: isValid()") {
    CBarCodeDataMatrix *test = new CBarCodeDataMatrix("11 11", 0);
    REQUIRE_FALSE(test->isValid());

    test = new CBarCodeDataMatrix("11 11", -1);
    REQUIRE_FALSE(test->isValid());
}*/
