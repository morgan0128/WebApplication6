import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserArt } from './user-art';

describe('UserArt', () => {
  let component: UserArt;
  let fixture: ComponentFixture<UserArt>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserArt]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserArt);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
