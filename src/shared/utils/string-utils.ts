/**
 * Replaces a substring starting from a given index
 * @param index Start index
 * @param replacement Replacement string
 * @param str Source string
 */
export const replaceAtIndex = (index: number, replacement: string, str: string): string => {
  return str.substr(0, index) + replacement + str.substr(index + replacement.length);
};
