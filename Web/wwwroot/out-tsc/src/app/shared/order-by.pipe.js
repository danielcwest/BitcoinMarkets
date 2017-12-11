"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var OrderByPipe = /** @class */ (function () {
    function OrderByPipe() {
    }
    OrderByPipe.prototype.transform = function (array, orderField, orderAscending, skip) {
        if (skip === void 0) { skip = false; }
        if (!skip) {
            array.sort(function (a, b) {
                var aField = a[orderField];
                var bField = b[orderField];
                var comparison = 0;
                //take no null value larger than null value. 
                //empty string is false, so we do not use the virable direct as the boolean expression
                if (aField == null && bField != null) {
                    comparison = 1;
                }
                else if (aField != null && bField == null) {
                    comparison = -1;
                }
                else {
                    if (typeof aField == "string")
                        comparison = aField.localeCompare(bField);
                    else
                        comparison = aField - bField;
                }
                return orderAscending ? comparison : comparison * -1;
            });
        }
        return array;
    };
    OrderByPipe = __decorate([
        core_1.Pipe({ name: 'orderBy' })
    ], OrderByPipe);
    return OrderByPipe;
}());
exports.OrderByPipe = OrderByPipe;
//# sourceMappingURL=order-by.pipe.js.map