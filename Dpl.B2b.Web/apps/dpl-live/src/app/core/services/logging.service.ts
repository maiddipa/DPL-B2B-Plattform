import { Injectable } from '@angular/core';

@Injectable()
export class LoggingService {
  error(message: string, ...args: any[]) {
    if (message === 'Undefined error') {
      if (
        args &&
        args.length > 0 &&
        args[0] &&
        args[0].rejection &&
        args[0].rejection.__proto__ &&
        args[0].rejection.__proto__.constructor &&
        args[0].rejection.__proto__.constructor.name === 'TypeError' &&
        args[0].rejection.__proto__.__proto__ &&
        args[0].rejection.__proto__.__proto__.constructor &&
        args[0].rejection.__proto__.__proto__.constructor.name === 'Error'
      ) {
        // console.log('remove error' + args);
      } else {
        console.error(message, ...args);
      }
    } else {
      console.error(message, ...args);
    }
  }

  warn(message: string, ...args: any[]) {
    console.warn(message, ...args);
  }

  info(message: string, ...args: any[]) {
    console.info(message, ...args);
  }

  log(message: string, ...args: any[]) {
    console.log(message, ...args);
  }

  message(type: LogType, message: string, ...args: any[]) {
    // Send errors to be saved here
    // The console.log is only for testing this example.
    switch (type) {
      case LogType.error:
        return console.error(message, ...args);
      case LogType.info:
        return console.info(message, ...args);
      case LogType.log:
        return console.log(message, ...args);
      case LogType.warn:
        return console.warn(message, ...args);
      default:
        return;
    }
  }
}

export enum LogType {
  error = 'error',
  info = 'info',
  log = 'log',
  warn = 'warn',
}
