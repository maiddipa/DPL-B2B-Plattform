import { PipeTransform } from '@angular/core';
import { LocalizationService } from '@app/core';

export abstract class TranslatePipeBase implements PipeTransform {
  abstract transform(value: any, ...args: any[]): any;
  constructor(private localizationService: LocalizationService) {}

  translate(value: string, name: string, fieldName: string = null): any {
    if (!name) {
      throw new Error('name cannot be null');
    }
    if (!value) {
      throw new Error('value cannot be null');
    }
    return this.localizationService.getTranslation(name, value, fieldName);
  }
}

export abstract class TranslatePipeEnumBase extends TranslatePipeBase {
  abstract name: string;
  transform(value: string) {
    return this.translate(value, this.name);
  }
}
