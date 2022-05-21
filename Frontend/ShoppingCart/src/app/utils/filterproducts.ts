import { IProductModel } from '../models/IProductModel';

export abstract class Filterproducts {
  static applyFilter(
    productCollection: IProductModel[],
    searchValue: string
  ): IProductModel[] {
    let filteredproductCollection: IProductModel[] = [];
    if (
      productCollection != null &&
      productCollection.length > 0 &&
      searchValue != ''
    ) {
      for (let product of productCollection) {
        if (
          product.ProductName.toLowerCase().includes(searchValue.toLowerCase())
        ) {
          filteredproductCollection.push(product);
        }
      }
    }
    return filteredproductCollection;
  }
}
