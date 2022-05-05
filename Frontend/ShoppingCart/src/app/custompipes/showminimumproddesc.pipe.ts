import { Pipe, PipeTransform } from '@angular/core';

/**
 * The custom pipe will return a portion of the product description length based on the input trim length.
 */
@Pipe({
  name: 'showminimumproddesc',
})
export class ShowminimumproddescPipe implements PipeTransform {
  transform(value: string, descriptionLengthToShow: number): string {
    let trasnformedDescription: string = value;
    let ellipsis = '...';
    let descriptionLengthToHide: number = 0;

    if (value != null && value.length > descriptionLengthToShow) {
      descriptionLengthToHide = value.length - descriptionLengthToShow;
      if (descriptionLengthToHide > 3) {
        trasnformedDescription = `${value.substring(
          0,
          descriptionLengthToShow
        )} ${ellipsis}`;
      }
      else
      {
        trasnformedDescription = `${value.substring(
          0,
          descriptionLengthToShow
        )}`;
      }
    }
    return trasnformedDescription;
  }
}
