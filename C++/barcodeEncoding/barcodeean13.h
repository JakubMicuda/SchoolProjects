/**

 * @Author Jakub Micuda(433715)
 */
#ifndef BARCODEEAN13_H
#define BARCODEEAN13_H

#include "BarCode.h"

class CBarCodeEAN13 : public CBarCode {

private:
    static const std::string RightCoding[];
    static const std::string LeftEvenCoding[];
    static const std::string LeftOddCoding[];
    static const std::string ParityEncodings[];

    int m_digits[12];
    int m_checksum;
    std::string coded[12];

    /**
     * @brief computeCheckSum
     * Computes checkSum, from the array of m_digits.
     * @return  Returns computed checkSum as integer.
     */
    int computeCheckSum() const;

    /**
     * @brief encode
     * Encodes array m_digits and m_checksum into 12 strings representing binary numbers of EAN13 barcode.
     * If there are invalid numbers given in m_digits (not from interval <0,9>), method returns.
     */
    void encode();

    /**
     * @brief recMake
     * Method prints svg rectangle by given arguments, on standard output.
     * @param offsetX   Sets "x" parameter of svg rectangle.
     * @param offsetY   Sets "y" parameter of svg rectangle.
     * @param width     Sets "width" parameter of svg recatngle.
     * @param height    Sets "height" parameter of svg rectangle.
     * @param color     Sets color of rectangle.
     */
    void recMake(size_t offsetX, size_t offsetY, size_t width, size_t height, std::string color) const;

    /**
     * @brief textMake
     * Method prints svg text by given arguments, on standard output.
     * @param offsetX   Sets "x" parameter of svg text.
     * @param offsetY   Sets "y" parameter of svg text.
     * @param scale     Sets "font-size" parameter of svg text.
     * @param number    Sets svg text itself (in this case it always is number).
     */
    void textMake(size_t offsetX, size_t offsetY, size_t scale, int number) const;

public:

    /**
     * @brief CBarCodeEAN13
     * Constructor of class CBarCodeEAN13 with (int*) parameter.
     * @param eanCode   Array of integers. If this parameter equals null, Constructor
     *                  calls zero() method and sets m_digits and m_checksum attribute with it.
     *                  Otherwise it fills m_digits with numbers from eanCode param.
     *                  Numbers on positions greater then 11 will be ignored.
     */
    CBarCodeEAN13(const int *eanCode);

    /**
     * @brief CBarCodeEAN13
     * Constructor of class CBarCodeEAN13 with string parameter.
     * @param eanCode   Parameter represents data as characters. parseInput() method is used to
     *                  transform eanCode in to array of numbers.
     */
    CBarCodeEAN13(const std::string& eanCode);

    /**
     * @brief zero
     * Method fills m_digits array with 0 and computes m_checksum with these numbers.
     * Then it encodes data into binary BarCode EAN13.
     */
    void zero();

    /**
     * @brief print
     * Method prints stored m_digits and m_checksum on standard output
     */
    void print() const;

    /**
     * @brief isValid
     * Method checks if m_digits is array of valid EAN13 numbers.
     * It also checks if stored m_checksum is actual.
     * @return  Returns true if m_digits is array of valid EAN13 numbers and m_checksum is actual.
     *          Otherwise, method returns false.
     */
    bool isValid() const;

    /**
     * @brief parseInput
     * Method transforms data from string to m_digits, and encode them into binary barcode EAN13 sequence.
     * @param data  String parameter, which is parsed into numbers. If it equals NULL or it contains invalid
     *              characters, method stops. If m_digits array is filled, next data characters are ignored.
     * @return      Returns true if whole m_digits array is filled and data contained only valid characters.
     *              Else returns false.
     */
    bool parseInput(const std::string& data);

    /**
     * @brief digits
     * Method returns actual value of m_digits array.
     * @return  m_digits.
     */
    int* digits();

    /**
     * @brief checkSum
     * Method returns actual value of m_checksum attribute.
     * @return m_checksum.
     */
    int checkSum() const;

    /**
     * @brief exportToSVG
     * Method prints encoded EAN13 barcode on standard output as svg file or just prints EAN13 barcode on
     * standard output as decimal numbers.
     * @param scale     Size_t parameter which sets scale of barcode.
     * @param offsetX   Size_t parameter which sets offset x of barcode.
     * @param offsetY   Size_t parameter which sets offset y of barcode.
     * @param svgHeader Bool parameter. If svgHeader= false, method prints EAN13 barcode as decimal numbers
     *                  on standard output.
     *                  Otherwise, method prints encoded EAN13 barcode on standard output as svg file.
     */
    void exportToSVG(std::size_t scale, std::size_t offsetX, std::size_t offsetY, bool svgHeader = false) const;
};

#endif // BARCODEEAN13_H
