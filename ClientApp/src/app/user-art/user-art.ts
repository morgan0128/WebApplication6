import {Component, effect, inject, OnInit, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
// import {DatePipe} from '@angular/common';
import {FormsModule} from '@angular/forms';
// import {ImageService, ImageDto} from '../services/ImageService';
// import { ImageService } from '../services/ImageService'
import { ImageLoader } from '../image-loader/image-loader'
import {ɵEmptyOutletComponent} from '@angular/router';
import {NgComponentOutlet, NgOptimizedImage} from '@angular/common';

interface ImageDTO {
  id: number;
  fileName: string;
  contentType: string;
  fileSize: number;
  storagePath: string;
  altText: string | null;
}

@Component({
  selector: 'app-user-art',
  imports: [FormsModule, NgOptimizedImage],
  templateUrl: './user-art.html',
  styleUrl: './user-art.css',
})
export class UserArt implements OnInit {
  private readonly http = inject(HttpClient);

  private readonly apiImageUrl = '/api/Image';

  imageLoader = new ImageLoader();

  urls: string[] = [];

  selectedFile: File | null = null;
  previewImageUrl: string | null = null;
  imageUploadError: string | null = null;
  imageIsUploading = false;

  protected readonly images = signal<ImageDTO[]>([]);

  ngOnInit(): void {
    // this.loadImageDTOs();
  }

  protected onFileSelected(event: Event): void {
    this.imageUploadError = null;

    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file){
      return;
    }

    const allowedTypes = ["image/jpeg", "image/png", "image/webp"];
    if (!allowedTypes.includes(file.type)) {
      this.imageUploadError = "Choos a JPG, PNG, or WebP image.";
      this.selectedFile = null;
      return;
    }

    const maxSizeMb = 5;
    if (file.size > maxSizeMb * 1024 * 1024) {
      this.imageUploadError = 'Image must be under ${maxSizeMb} MB.}'
      this.selectedFile = null;
      return;
    }

    this.selectedFile = file;
    this.previewImageUrl = URL.createObjectURL(file);

  }

  protected uploadImage(): void {
    if (!this.selectedFile) {
      return;
    }

    const formData: FormData = new FormData();
    formData.append('file', this.selectedFile);

    this.imageIsUploading = true;

    this.http.post(this.apiImageUrl, formData).subscribe({
      next: (image) => {
        console.log('Uploaded:', image)
        this.imageIsUploading = false;
      },
      error: () => {
        this.imageUploadError = 'Upload failed.';
        this.imageIsUploading = false;
      }
    });
  }

  // protected deleteImage(image: Image): void {
  //   this.http.delete(`${this.apiImageUrl}/${image.id}`).subscribe({
  //     next: () => {
  //       this.images.update((images) => images.filter((item) => item.id !== image.id));
  //     },
  //     error: () => this.imageError.set('The item could not be deleted.')
  //   });
  // }

  protected readonly ImageLoader = ImageLoader;
}
