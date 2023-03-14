/* eslint-disable @typescript-eslint/no-explicit-any */

/**
 * Returns whether the passed array is an array with valid values
 * @param array Array object
 */
export const isArrayNullOrEmpty = (array: any[]): boolean => {
  if (!array || !Array.isArray(array) || !array.length) return true;

  return false;
};
