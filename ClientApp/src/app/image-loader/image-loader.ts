import {Component, inject, OnInit, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs'
import {AnyCatcher} from 'rxjs/internal/AnyCatcher';
import {NgOptimizedImage} from '@angular/common';

interface ImageUrlDTO1{
  id: number,
  fileContent: Blob
}
interface ImageUrlDTO2{
  id: number,
  url: string
}

@Component({
  selector: 'app-image-loader',
  imports: [],
  templateUrl: './image-loader.html',
  styleUrl: './image-loader.css',
})
export class ImageLoader implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly apiImageId5Url = '/api/Image';

  protected readonly displayingAll = signal<boolean>(false);

  imageUrls: string[] = ([]);

  imageIdList: number[] = [];

  protected readonly imagesLoaded = signal<boolean>(false);

  ngOnInit() {
    // this.loadImage();
    this.loadImages();

  }

  displayAll() {
    let switched = !this.displayingAll();
    this.displayingAll.set(switched)
  }

  loadImages(): void {
    this.generateData().subscribe({

      next: list => {
        this.imageIdList = list;
        const pathPref = 'api/Image/';

        this.imageIdList.forEach(value => {
          let path = pathPref + value;
          this.http.get(path, {responseType: 'blob'}).subscribe({
            next: blob => {
              this.imageUrls.push(URL.createObjectURL(blob));
            },
            error: () => {
              console.log('Failed to load images');
            }
          });
        })

        this.imagesLoaded.set(true);
      }
    });

  }

  generateData(){
    return this.http.get<number[]>('api/Image');
  }

}
