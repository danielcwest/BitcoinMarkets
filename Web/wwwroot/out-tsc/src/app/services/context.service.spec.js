"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var context_service_1 = require("./context.service");
describe('ContextService', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [context_service_1.ContextService]
        });
    });
    it('should be created', testing_1.inject([context_service_1.ContextService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=context.service.spec.js.map