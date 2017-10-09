import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'commaSeparatedNumber' })
export class CommaSeparatedNumberPipe implements PipeTransform {
    transform(value: number, args: string[]): any {
        return value.toLocaleString();
    }
}