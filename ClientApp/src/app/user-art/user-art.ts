import {Component, inject, signal} from '@angular/core';
import { HttpClient } from '@angular/common/http';
// import {DatePipe} from '@angular/common';
import {FormsModule} from '@angular/forms';

interface Image {
  id: number;
  fileName: string;
  contentType: string;
  fileSize: bigint;
  storagePath: string;
  altText: string;
}

@Component({
  selector: 'app-user-art',
  imports: [FormsModule],
  templateUrl: './user-art.html',
  styleUrl: './user-art.css',
})
export class UserArt {
  private readonly http = inject(HttpClient);

  private readonly apiImageUrl = '/api/Image';

  selectedFile: File | null = null;
  previewUrl: string | null = null;
  imageError2: string | null = null;
  isUploading = false;

  protected readonly images = signal<Image[]>([]);

  protected onFileSelected(event: Event): void {
    this.imageError2 = null;

    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file){
      return;
    }

    const allowedTypes = ["image/jpeg", "image/png", "image/webp"];
    if (!allowedTypes.includes(file.type)) {
      this.imageError2 = "Choos a JPG, PNG, or WebP image.";
      this.selectedFile = null;
      return;
    }

    const maxSizeMb = 5;
    if (file.size > maxSizeMb * 1024 * 1024) {
      this.imageError2 = 'Image must be under ${maxSizeMb} MB.}'
      this.selectedFile = null;
      return;
    }

    this.selectedFile = file;
    this.previewUrl = URL.createObjectURL(file);

  }

  protected uploadImage(): void {
    if (!this.selectedFile) {
      return;
    }

    const formData: FormData = new FormData();
    formData.append('file', this.selectedFile);

    this.isUploading = true;

    this.http.post(this.apiImageUrl, formData).subscribe({
      next: (image) => {
        console.log('Uploaded:', image)
        this.isUploading = false;
      },
      error: () => {
        this.imageError2 = 'Upload failed.';
        this.isUploading = false;
      }
    });
  }

}
