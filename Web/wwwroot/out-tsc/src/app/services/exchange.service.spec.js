"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var exchange_service_1 = require("./exchange.service");
describe('ExchangeService', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [exchange_service_1.ExchangeService]
        });
    });
    it('should be created', testing_1.inject([exchange_service_1.ExchangeService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=exchange.service.spec.js.map