#include <iostream>
#include <fstream>
#include <string>

#include "BarCodeEAN13.h"
#include "BarCodeDataMatrix.h"

using namespace std;

int main(int argc, char *argv[])
{
    int eanArray[12] = {9, 7, 8, 8, 0, 2, 1, 0, 4, 8, 5, 7};

    CBarCodeEAN13 eanFromArray(eanArray);
    CBarCodeEAN13 eanFromString("330721166750");
    CBarCodeDataMatrix *dataMatrix = new CBarCodeDataMatrix("101011 100101 110100 101011 100010 111111", 6);

     //exportToSVG() basic tests
        cout << CBarCode::XMLVersion << endl;
        cout << CBarCode::Doctype << endl;
        cout << CBarCode::SVGHeader << endl;

        eanFromArray.exportToSVG(4, 40, 60);
        eanFromString.exportToSVG(5, 500, 60);
        dataMatrix->exportToSVG(40, 40, 400);

        cout << "</svg>" << endl;

        delete dataMatrix;

    return 0;
}

