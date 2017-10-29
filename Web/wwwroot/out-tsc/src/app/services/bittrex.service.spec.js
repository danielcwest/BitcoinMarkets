"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var bittrex_service_1 = require("./bittrex.service");
describe('BittrexService', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [bittrex_service_1.BittrexService]
        });
    });
    it('should be created', testing_1.inject([bittrex_service_1.BittrexService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=bittrex.service.spec.js.map