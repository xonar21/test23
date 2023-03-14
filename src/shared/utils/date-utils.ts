/**
 * Returns the difference in minutes between two Date objects
 * @param date1 First date object
 * @param date2 Second date object
 */
export const diffByMinutes = (date1: Date, date2: Date): number => {
  let diff = (date2.getTime() - date1.getTime()) / 1000;
  diff /= 60;
  return Math.abs(Math.round(diff));
};
