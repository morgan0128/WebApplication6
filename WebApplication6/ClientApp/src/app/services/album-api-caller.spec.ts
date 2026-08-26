import { TestBed } from '@angular/core/testing';

import { AlbumApiCaller } from './album-api-caller';

describe('AlbumApiCaller', () => {
  let service: AlbumApiCaller;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AlbumApiCaller);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
