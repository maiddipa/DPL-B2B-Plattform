import { ArgumentOutOfRangeError } from 'rxjs';

export const NOT_IMPLEMENTED_EXCEPTION: Error = new Error(
  'Not implemented exception'
);
export const ARGUMENT_OUT_OF_RANGE = (message: string) => {
  const error = new ArgumentOutOfRangeError();
  error.message = message;
  return error;
};
new Error('Not implemented exception');
