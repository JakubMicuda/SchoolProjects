/**
 * @Author Jakub Micuda(433715)
 */
#ifndef BARCODEDATAMATRIX_H
#define BARCODEDATAMATRIX_H

#include "BarCode.h"

class CBarCodeDataMatrix : public CBarCode {

private:
    bool** m_data;
    std::size_t m_size;

    /**
     * @brief recMake
     * Method prints svg rectangle on standard output by given parameters.
     * @param offsetX   Sets "x" parameter of svg rectangle.
     * @param offsetY   Sets "y" parameter of svg rectangle.
     * @param scale     Sets "width" and "height" parameter of svg rectangle.
     * @param data      Sets color of rectangle (true=black, false=white).
     */
    void recMake(std::size_t offsetX, std::size_t offsetY, std::size_t scale, bool data) const;

public:

    /**
     * @brief CBarCodeDataMatrix
     * Constructor of CBarCodeDataMatrix class.
     * @param data              Sequence of '0'/'1' characters.
     * @param dataMatrixSize    Size of m_data matrix.
     */
    CBarCodeDataMatrix(const std::string& data, std::size_t dataMatrixSize);

    /**
     * @brief size
     * Method returns actual m_size attribute.
     * @return Returns m_size.
     */
    std::size_t size() const;

    /**
     * @brief zero
     * Fills matrix with false
     */
    void zero();

    /**
     * @brief print
     * Prints matrix on standard output with true as '#' and false as ' '.
     */
    void print() const;

    /**
     * @brief isValid
     * Checks if m_data allocated succesfully
     * @return True if m_data was succesfully allocated
     *         otherwise, it returns false
     */
    bool isValid() const;

    /**
     * @brief parseInput
     * Parsing input string into m_data matrix, where '1' represents true and '0' represent false.
     * @param data  Input sequence of '1' and '0' characters
     * @return  True if parsing was succesful. In case of invalid characters or not filling whole matrix
     *          method returns false
     */
    bool parseInput(const std::string& data);

    /**
     * @brief exportToSVG
     * Prints DataMatrix on standard output as svg file.
     * @param scale     Sets "width" and "height" of rectangle.
     * @param offsetX   Sets offset x of barcode.
     * @param offsetY   Sets offset y of barcode.
     * @param svgHeader If this parameter is true, method prints whole DataMatrix as svg file
     *                  Otherwise it prints barcode on standard output (not as svg file)
     */
    void exportToSVG(std::size_t scale, std::size_t offsetX, std::size_t offsetY, bool svgHeader = false) const;

    ~CBarCodeDataMatrix();
};

#endif // BARCODEDATAMATRIX_H
