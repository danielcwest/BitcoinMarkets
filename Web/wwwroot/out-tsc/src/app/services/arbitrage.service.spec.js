"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var arbitrage_service_1 = require("./arbitrage.service");
describe('ArbitrageService', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [arbitrage_service_1.ArbitrageService]
        });
    });
    it('should be created', testing_1.inject([arbitrage_service_1.ArbitrageService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=arbitrage.service.spec.js.map