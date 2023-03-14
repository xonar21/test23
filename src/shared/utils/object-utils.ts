/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/explicit-module-boundary-types */

/**
 * Returns whether the objects are deep equal or not
 * @param x First object
 * @param y Second object
 */
export const deepEqual = (x: any, y: any): boolean => {
  const ok = Object.keys,
    tx = typeof x,
    ty = typeof y;
  return x && y && tx === "object" && tx === ty
    ? ok(x).length === ok(y).length && ok(x).every(key => deepEqual(x[key], y[key]))
    : x === y;
};

/**
 * Returns the key of the object based on the passed value
 * @param obj Object source
 * @param value Value of the object to compare with
 */
export const getKeyByValue = (obj: any, value: any) => {
  return Object.keys(obj).find(key => deepEqual(obj[key], value));
};

/**
 * Returns the value of an object based on passed string path
 * @param obj Object source
 * @param path Value path string or path array
 * @param separator Property separator (used for split function)
 */
export const getValueByPath = (obj: any, path: string | string[], separator = ".") => {
  if (!Array.isArray(path)) path = path.split(separator);
  for (let i = 0, len = path.length; i < len; i++) {
    obj = obj[path[i]];
  }
  return obj;
};

/**
 * Returns the path of an object value
 * @param obj Object source
 * @param value Target value
 */
export const getValuePath = (obj: any, value: any) => {
  const path: string[] = [];
  let found = false;

  function search(haystack: any) {
    for (const key in haystack) {
      path.push(key);
      if (haystack[key] === value) {
        found = true;
        break;
      }
      if (haystack[key].constructor === Object) {
        search(haystack[key]);
        if (found) break;
      }
      path.pop();
    }
  }

  search(obj);
  return path;
};
