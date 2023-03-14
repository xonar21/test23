export const REQUEST_TYPES = ["get", "post", "patch", "put", "delete"] as const;
type RequestTypesTuple = typeof REQUEST_TYPES;
export type RequestType = RequestTypesTuple[number];
