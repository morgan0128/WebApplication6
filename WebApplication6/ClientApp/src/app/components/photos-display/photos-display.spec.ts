import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotosDisplay } from './photos-display';

describe('PhotosDisplay', () => {
  let component: PhotosDisplay;
  let fixture: ComponentFixture<PhotosDisplay>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PhotosDisplay]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PhotosDisplay);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
