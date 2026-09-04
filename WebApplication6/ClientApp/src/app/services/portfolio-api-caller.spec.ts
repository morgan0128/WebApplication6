import { TestBed } from '@angular/core/testing';

import { PortfolioApiCaller } from './portfolio-api-caller';

describe('PortfolioApiCaller', () => {
  let service: PortfolioApiCaller;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PortfolioApiCaller);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
