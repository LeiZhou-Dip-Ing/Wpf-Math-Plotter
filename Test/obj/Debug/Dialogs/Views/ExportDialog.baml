   M S B A M L   `   `   `  ����?  ;Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Test.Dialogs.ViewsTest.Dialogs.Views  5      %    Test.Dialogs.Views.ExportDialog   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:Test.Dialogs.Views     5      .��5
      �   � ColumnDefinitions  5   
   �� 5      $��2* �6   �� 5      $��3* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   $��0��6   .+�5      �� $��10q�6   ��5      �� 5      $��Settings��6    $
��Bold"�5       .��5      ��  �� Orientation$ Vertical=�6   $��10q�64   .+�5      �� $ 
Horizontal=�6!   $
��Left=�6:   .+�5       �� $	��300��5!   "   $��0,0,5,0q�6.    �� 
IsReadOnly . ��5    "   �� *
SavePath+ �� UpdateSourceTrigger$ PropertyChanged=�5!   @   �� 5"      $��Browse��6!   $��75��6T   ��62   �� *BrowseCommand+6T   5#      �� 5&      $ 
Horizontal=�6!   $��0,10,0,5q�6:   .+�5'      �� $��Width:��6$   $��Center=�62   $��0,0,5,0q�6M   �� 5(      $	��100��5+   1   $��0,0,20,0q�6=      ValidationErrorTemplate =� ErrorTemplate# [  5,   1   ��5(   "   �� *
ImageWidth+$ PropertyChanged=�" �� ValidatesOnNotifyDataErrors . 5,   1   �� 5.      $��Height:��6$   $��Center=�63   $��0,0,5,0q�6N   �� 5/      $��90��52   1   # [  53   1   ��5/   "   �� *ImageHeight+$ PropertyChanged=� . 53   1   54      55      56      �� 59      $ 
Horizontal=�6   $
��Left=�62   $��Bottom=�6M   $��10q�6h   .+�5:      �� $��Confirm��6<   $��0,0,10,0q�6N   ��6   �� *ConfirmCommand+6N   �� 5;      $��Cancel��6;   ��6   �� *CancelCommand+6;   5<      5=      �� 5@   
   $��SVG Preview��6   $
��Bold"�6(   $��1��6:   $��0��6J   $��10q�6W   .��5A      �� $
��Auto=�6   $
��Auto=�6@   $��10q�6c   .��5C      �� $��Uniform=�5F      ��5C      �� *SvgPreviewImage+��5D      �� *
ImageWidth+$ PropertyChanged=���5E      �� *ImageHeight+$ PropertyChanged=�5F      5G      5H      5I      5J      