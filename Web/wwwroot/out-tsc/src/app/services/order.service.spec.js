"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var order_service_1 = require("./order.service");
describe('OrderService', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [order_service_1.OrderService]
        });
    });
    it('should be created', testing_1.inject([order_service_1.OrderService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=order.service.spec.js.map