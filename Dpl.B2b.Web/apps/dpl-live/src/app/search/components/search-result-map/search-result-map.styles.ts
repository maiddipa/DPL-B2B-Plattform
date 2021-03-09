// taken from https://snazzymaps.com/style/28/bluish
export const mapStyles = [
  {
    stylers: [
      {
        hue: '#007fff',
      },
      {
        saturation: 89,
      },
    ],
  },
  {
    featureType: 'water',
    stylers: [
      {
        color: '#ffffff',
      },
    ],
  },
  {
    featureType: 'administrative.country',
    elementType: 'labels',
    stylers: [
      {
        visibility: 'off',
      },
    ],
  },
];
