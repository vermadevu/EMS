import { Component, forwardRef, inject, signal } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { UploadService } from '../../../core/services/upload-service';
import { finalize } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-image-upload',
  standalone: true,
  imports: [
    MatIconModule
  ],
  templateUrl: './image-upload.html',
  styleUrl: './image-upload.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ImageUploadComponent),
      multi: true
    }
  ]
})
export class ImageUploadComponent implements ControlValueAccessor {
  private readonly uploadService = inject(UploadService);
  readonly imageUrl = signal<string | null>(null);
  readonly uploading = signal(false);
  readonly disabled = signal(false);

  onChange: (value: string | null) => void = () => { };

  onTouched: () => void = () => { };

  writeValue(value: string | null): void {
    this.imageUrl.set(value);
  }

  registerOnChange(fn: (value: string | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(disabled: boolean): void {
    this.disabled.set(disabled);
  }


  selectImage(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    const allowedTypes = [
      'image/png',
      'image/jpeg',
      'image/webp'
    ];

    if (!allowedTypes.includes(file.type)) {
      alert('Only JPG, PNG and WEBP images are allowed.');
      input.value = '';
      return;
    }

    if (file.size > 25 * 1024 * 1024) {
      alert('Maximum image size is 5 MB.');
      input.value = '';
      return;
    }

    this.upload(file);
  }

  private upload(file: File): void {
    this.uploading.set(true);

    this.uploadService
      .uploadImage(file)
      .pipe(
        finalize(() => this.uploading.set(false))
      )
      .subscribe({
        next: response => {
          this.imageUrl.set(response.url);
          this.onChange(response.url);
          this.onTouched();
        },
        error: error => {
          console.error(error);
        }
      });
  }

  remove(): void {
  this.imageUrl.set(null);
  this.onChange(null);
  this.onTouched();
}

}