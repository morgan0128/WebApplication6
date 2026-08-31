import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioManager } from './portfolio-manager';

describe('PortfolioManager', () => {
  let component: PortfolioManager;
  let fixture: ComponentFixture<PortfolioManager>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PortfolioManager]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PortfolioManager);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
