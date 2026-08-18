import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkQueue } from './work-queue';

describe('WorkQueue', () => {
  let component: WorkQueue;
  let fixture: ComponentFixture<WorkQueue>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkQueue]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkQueue);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
