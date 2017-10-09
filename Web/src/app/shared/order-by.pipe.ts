import { Pipe, PipeTransform } from "@angular/core";
@Pipe({ name: 'orderBy' })
export class OrderByPipe implements PipeTransform {
    transform(array: Array<any>, orderField: string, orderAscending: boolean, skip: boolean = false): Array<any> {
        if (!skip) {
            array.sort((a: any, b: any) => {
                let aField = a[orderField];
                let bField = b[orderField];
                let comparison = 0;
                //take no null value larger than null value. 
                //empty string is false, so we do not use the virable direct as the boolean expression
                if (aField == null && bField != null) {
                    comparison = 1;
                }
                else if (aField != null && bField == null) {
                    comparison = -1;
                }
                else {
                    if (typeof aField == "string") comparison = aField.localeCompare(bField);
                    else comparison = aField - bField;

                }
                return orderAscending ? comparison : comparison * -1;
            });
        }
        return array;
    }
}