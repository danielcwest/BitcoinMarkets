import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'btcPrice'
})
export class BtcPricePipe implements PipeTransform {

    transform(value: any, args?: any): any {

        if (Number.isNaN(value)) {
            return null;
        }
        return Number(value).toFixed(8);
    }

}
