import {Component, inject, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { UserArt } from '../user-art/user-art';
import {NgOptimizedImage} from '@angular/common';


interface ImageUrlDTO{
  url: string,
  width: number,
  height: number
}

@Component({
  selector: 'app-image-loader',
  imports: [],
  templateUrl: './image-loader.html',
  styleUrl: './image-loader.css',
})
export class ImageLoader implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly apiImageId5Url = '/api/Image/5';

  // imageUrl: string | null = null;

  imageUrls: string[] = ([]);

  ngOnInit() {
    this.loadImage();
  }

  loadImage(): void {

    this.http.get(this.apiImageId5Url, {responseType: 'blob'}).subscribe({
      next: blob => {
        this.imageUrls.push(URL.createObjectURL(blob));
      },
      error: () => {
        console.log('Could not load image');
      }
    })
  }

  // loadImageById(id: number): void {
  //   var path = this.apiImageUrl + '/' + id;
  //   this.http.get(path, {responseType: 'blob'}).subscribe({
  //     next: blob => {
  //       this.imageUrls.push(URL.createObjectURL(blob));
  //     },
  //     error: () => {
  //       console.log('Could not load image');
  //     }
  //   })
  // }

  // loadImage(): string | null{
  //   this.http.get(this.apiImageUrl, {responseType: 'blob'}).subscribe({
  //     next: blob => {
  //       return URL.createObjectURL(blob);
  //     },
  //     error: () => {
  //       console.log('Could not load image');
  //       return null;
  //     },
  //     complete: () => {
  //       return null;
  //     }
  //   })
  // }

}
