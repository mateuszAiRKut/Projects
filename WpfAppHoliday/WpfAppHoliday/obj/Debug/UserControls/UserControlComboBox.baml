   M S B A M L   `   `   `  ����H  DWpfAppHoliday, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:WpfAppHoliday.UserControlsWpfAppHoliday.UserControls  5      4    .WpfAppHoliday.UserControls.UserControlComboBox   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:WpfAppHoliday.UserControls     5      .��5	      �� -   5          ��Name$  MultiSelectCombo��5
   	    J� SnapsToDevicePixels .  � OverridesDefaultStyle . $
��Auto=�5   	   $
��Auto=�5   	   ��. $ �� IsSynchronizedWithCurrentItem$
 TrueY�5   	   ��5   
   �� 5      .R�5      �� -   5        ��5      �� *Title+ d� 	IsChecked 5      ��  �� Path$ 
IsSelected� �� Mode$ TwoWay=�
 � Tag 5      �� *FindAncestor+ �� AncestorType" ��5      5      ��5   
   �� 5      	 �� 
TargetType"	 ��6   .R�5      � .+�5      d� $��ToggleButton��6#   $��2��5      |�.  
 �� 	ClickMode$
 Press=�5      ��5   7   �� $
 Text�$ TwoWay=� �� RelativeSource ��  �� Mode$ FindAncestor=�$ UserControlL� 5       �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+5!      � 5"      $��Popup��5#      $��Right=�5$       � AllowsTransparency . |�.   �� IsDropDownOpen#��z 5'      $��Slide=�5(      .��5)      � $��DropDown��66   
������� .  � ActualWidth#��z 6a    �� MaxDropDownHeight#��z 5*   #   .+�5+      ��  ��Name$ DropDownBorder��6%   $��1q�6=   
���   �6Q   �� 5,      $��4,6,4,6q�6+    .  � DataContext 6W   �� .��5-   "   ��  K� IsItemsHost . -  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation$ 	Contained=�6@   5.      5/      50      51       �� Triggers 52      P� 53       P� Property �@��HasItems P� Value .  . �54      ��  �� 
TargetName$ DropDownBorder��6!    �� Property � �� �� Value$ 95��6R   55      P� 56       P� 
SourceName$ Popup��6    �@�AllowsTransparency . . �57      �� $ DropDownBorder��6!    �@��CornerRadius$ 4��6W   �� 58      $ DropDownBorder��6!    � ��$ 0,2,0,0q�6O   59      5;      5=      5>      