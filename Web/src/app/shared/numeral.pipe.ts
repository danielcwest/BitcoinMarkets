import { Pipe, PipeTransform } from '@angular/core';
import * as numeral from 'numeral';

@Pipe({
    name: 'numeral'
})
export class NumeralPipe implements PipeTransform {

    transform(value: number, format: string): any {
        return numeral(value).format(format);
    }

}
