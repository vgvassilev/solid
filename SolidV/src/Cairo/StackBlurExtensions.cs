/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using Cairo;

namespace SolidV.Cairo
{
  /// <summary>
  /// ...
  /// </summary>
  using Cairo = global::Cairo;
  public static class ContextExtensions_BlurSurface
  {
/*    private static void StackBlur(Context context, Context input_context, int rx, int ry) {
    guint32 *input_pixels;
    guint32 *output_pixels;
    int w, h, wm, hm, wh, div;
    int *r, *g, *b, *a;

    int rsum,gsum,bsum,asum,x,y,i,p,yp,yi,yw;
    int *vmin;
    int divsum;

    yw=yi=0;

    int *stack;
    int stackpointer;
    int stackstart;
    int *sir;
    int rbs;
    int r1;
    int routsum,goutsum,boutsum, aoutsum;
    int rinsum,ginsum,binsum, ainsum;
    int rowstride;

    g_return_if_fail (rx > 0 || ry > 0);

    input_pixels = (guint32 *) cairo_image_surface_get_data (input);
    output_pixels = (guint32 *) cairo_image_surface_get_data (output);
    rowstride = cairo_image_surface_get_stride (input);
    w = cairo_image_surface_get_width (input);
    h = cairo_image_surface_get_height (input);

    g_return_if_fail (cairo_image_surface_get_width (output) == w);
    g_return_if_fail (cairo_image_surface_get_height (output) == h);
    g_return_if_fail (cairo_image_surface_get_stride (output) == rowstride);

    wm = w - 1;
    hm = h - 1;
    wh = w * h;

    r = g_new (int, wh);
    g = g_new (int, wh);
    b = g_new (int, wh);
    a = g_new (int, wh);

    div = 2 * rx + 1;
    divsum = (div + 1) >> 1; 
    divsum *= divsum;
    stack = g_new (int, div * 4);
    r1 = rx + 1;

    vmin = g_new (int, w);
    for (y =0; y < h; y++) {
	rinsum=ginsum=binsum=ainsum=routsum=goutsum=boutsum=aoutsum=rsum=gsum=bsum=asum=0;
	yi = y * rowstride / 4;

	for (i = -rx; i <= rx; i++) {
	    p = input_pixels [yi + MIN (wm, MAX (i, 0))];
	    sir = &stack [4 * (i + rx)];
	    sir[0]=(p & 0x00ff0000)>>16;
	    sir[1]=(p & 0x0000ff00)>>8;
	    sir[2]=(p & 0x000000ff);
	    sir[3]=(p & 0xff0000ff)>>24;
	    rbs= r1 - ABS (i);
	    rsum+=sir[0]*rbs;
	    gsum+=sir[1]*rbs;
	    bsum+=sir[2]*rbs;
	    asum+=sir[3]*rbs;
	    if (i>0){
		rinsum+=sir[0];
		ginsum+=sir[1];
		binsum+=sir[2];
		ainsum+=sir[3];
	    } else {
		routsum+=sir[0];
		goutsum+=sir[1];
		boutsum+=sir[2];
		aoutsum+=sir[3];
	    }
	}
	stackpointer=rx;

	for (x=0;x<w;x++){

	    r[yi] = rsum / divsum;
	    g[yi] = gsum / divsum;
	    b[yi] = bsum / divsum;
	    a[yi] = asum / divsum;
	    rsum-=routsum;
	    gsum-=goutsum;
	    bsum-=boutsum;
	    asum-=aoutsum;

	    stackstart=stackpointer-rx+div;
	    sir = &stack [4 * (stackstart % div)];

	    routsum-=sir[0];
	    goutsum-=sir[1];
	    boutsum-=sir[2];
	    aoutsum-=sir[3];

	    if(y==0){
		vmin[x]=MIN (x + rx + 1, wm);
	    }
	    p = input_pixels [yw + vmin[x]];

	    sir[0]=(p & 0x00ff0000)>>16;
	    sir[1]=(p & 0x0000ff00)>>8;
	    sir[2]=(p & 0x000000ff);
	    sir[3]=(p & 0xff000000)>>24;

	    rinsum+=sir[0];
	    ginsum+=sir[1];
	    binsum+=sir[2];
	    ainsum+=sir[3];

	    rsum+=rinsum;
	    gsum+=ginsum;
	    bsum+=binsum;
	    asum+=ainsum;

	    stackpointer = (stackpointer + 1) % div;
	    sir = &stack [4 * ((stackpointer) % div)];

	    routsum+=sir[0];
	    goutsum+=sir[1];
	    boutsum+=sir[2];
	    aoutsum+=sir[3];

	    rinsum-=sir[0];
	    ginsum-=sir[1];
	    binsum-=sir[2];
	    ainsum-=sir[3];

	    yi++;
	}
	yw+=w;
    }
    g_free (vmin);
    g_free (stack);

    div = 2 * ry + 1;
    divsum = (div + 1) >> 1;
    divsum *= divsum;

    stack = g_new0 (int, div * 4);
    r1 = ry + 1;

    vmin = g_new0 (int, h);
    for (x=0;x<w;x++){
	rinsum=ginsum=binsum=ainsum=routsum=goutsum=boutsum=aoutsum=rsum=gsum=bsum=asum=0;
	yp=-ry*w;
	for(i=-ry;i<=ry;i++){
	    yi= MAX(0,yp)+x;

	    sir = &stack [4 * (i + ry)];

	    sir[0]=r[yi];
	    sir[1]=g[yi];
	    sir[2]=b[yi];
	    sir[3]=a[yi];

	    rbs=r1 - ABS(i);

	    rsum+=r[yi]*rbs;
	    gsum+=g[yi]*rbs;
	    bsum+=b[yi]*rbs;
	    asum+=a[yi]*rbs;

	    if (i>0){
		rinsum+=sir[0];
		ginsum+=sir[1];
		binsum+=sir[2];
		ainsum+=sir[3];
	    } else {
		routsum+=sir[0];
		goutsum+=sir[1];
		boutsum+=sir[2];
		aoutsum+=sir[3];
	    }

	    if(i<hm){
		yp+=w;
	    }
	}
	yi=x;
	stackpointer=ry;
	for (y=0;y<h;y++){
	    output_pixels [yi] = 
		((asum / divsum) << 24) |
		((rsum / divsum) << 16) |
		((gsum / divsum) << 8) |
		(bsum / divsum);
	    rsum-=routsum;
	    gsum-=goutsum;
	    bsum-=boutsum;
	    asum-=aoutsum;

	    stackstart=stackpointer-ry+div;
	    sir = &stack [4 * (stackstart % div)];

	    routsum-=sir[0];
	    goutsum-=sir[1];
	    boutsum-=sir[2];
	    aoutsum-=sir[3];

	    if(x==0) {
		vmin[y] = MIN(y+r1,hm)*w;
	    }
	    p=x+vmin[y];

	    sir[0]=r[p];
	    sir[1]=g[p];
	    sir[2]=b[p];
	    sir[3]=a[p];

	    rinsum+=sir[0];
	    ginsum+=sir[1];
	    binsum+=sir[2];
	    ainsum+=sir[3];

	    rsum+=rinsum;
	    gsum+=ginsum;
	    bsum+=binsum;
	    asum+=ainsum;

	    stackpointer=(stackpointer+1)%div;
	    sir = &stack[4 * stackpointer];

	    routsum+=sir[0];
	    goutsum+=sir[1];
	    boutsum+=sir[2];
	    aoutsum+=sir[3];

	    rinsum-=sir[0];
	    ginsum-=sir[1];
	    binsum-=sir[2];
	    ainsum-=sir[3];

	    yi+= rowstride / 4;
	}
    }
    g_free (vmin);
    g_free (stack);

    g_free (r);
    g_free (g);
    g_free (b);
    g_free (a);
    }
*/
  }
}




/*
 * Copyright © 2008 Kristian Høgsberg
 * Copyright © 2009 Chris Wilson
 *
 * Permission to use, copy, modify, distribute, and sell this software and its
 * documentation for any purpose is hereby granted without fee, provided that
 * the above copyright notice appear in all copies and that both that copyright
 * notice and this permission notice appear in supporting documentation, and
 * that the name of the copyright holders not be used in advertising or
 * publicity pertaining to distribution of the software without specific,
 * written prior permission.  The copyright holders make no representations
 * about the suitability of this software for any purpose.  It is provided "as
 * is" without express or implied warranty.
 *
 * THE COPYRIGHT HOLDERS DISCLAIM ALL WARRANTIES WITH REGARD TO THIS SOFTWARE,
 * INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS, IN NO
 * EVENT SHALL THE COPYRIGHT HOLDERS BE LIABLE FOR ANY SPECIAL, INDIRECT OR
 * CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM LOSS OF USE,
 * DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER
 * TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE
 * OF THIS SOFTWARE.
 */
/*
#include <math.h>
#include <stdint.h>

#define ARRAY_LENGTH(a) (sizeof (a) / sizeof (a)[0])

// Performs a simple 2D Gaussian blur of radius @radius on surface @surface.
void
blur_image_surface (cairo_surface_t *surface, int radius)
{
    cairo_surface_t *tmp;
    int width, height;
    int src_stride, dst_stride;
    int x, y, z, w;
    uint8_t *src, *dst;
    uint32_t *s, *d, a, p;
    int i, j, k;
    uint8_t kernel[17];
    const int size = ARRAY_LENGTH (kernel);
    const int half = size / 2;

    if (cairo_surface_status (surface))
    return;

    width = cairo_image_surface_get_width (surface);
    height = cairo_image_surface_get_height (surface);

    switch (cairo_image_surface_get_format (surface)) {
    case CAIRO_FORMAT_A1:
    default:
    // Don't even think about it!
    return;

    case CAIRO_FORMAT_A8:
    //* Handle a8 surfaces by effectively unrolling the loops by a
    //* factor of 4 - this is safe since we know that stride has to be a
    //* multiple of uint32_t.
    width /= 4;
    break;

    case CAIRO_FORMAT_RGB24:
    case CAIRO_FORMAT_ARGB32:
    break;
    }

    tmp = cairo_image_surface_create (CAIRO_FORMAT_ARGB32, width, height);
    if (cairo_surface_status (tmp))
    return;

    src = cairo_image_surface_get_data (surface);
    src_stride = cairo_image_surface_get_stride (surface);

    dst = cairo_image_surface_get_data (tmp);
    dst_stride = cairo_image_surface_get_stride (tmp);

    a = 0;
    for (i = 0; i < size; i++) {
    double f = i - half;
    a += kernel[i] = exp (- f * f / 30.0) * 80;
    }

    // Horizontally blur from surface -> tmp
    for (i = 0; i < height; i++) {
    s = (uint32_t *) (src + i * src_stride);
    d = (uint32_t *) (dst + i * dst_stride);
    for (j = 0; j < width; j++) {
        if (radius < j && j < width - radius) {
        d[j] = s[j];
        continue;
        }

        x = y = z = w = 0;
        for (k = 0; k < size; k++) {
        if (j - half + k < 0 || j - half + k >= width)
            continue;

        p = s[j - half + k];

        x += ((p >> 24) & 0xff) * kernel[k];
        y += ((p >> 16) & 0xff) * kernel[k];
        z += ((p >>  8) & 0xff) * kernel[k];
        w += ((p >>  0) & 0xff) * kernel[k];
        }
        d[j] = (x / a << 24) | (y / a << 16) | (z / a << 8) | w / a;
    }
    }

    // Then vertically blur from tmp -> surface
    for (i = 0; i < height; i++) {
    s = (uint32_t *) (dst + i * dst_stride);
    d = (uint32_t *) (src + i * src_stride);
    for (j = 0; j < width; j++) {
        if (radius <= i && i < height - radius) {
        d[j] = s[j];
        continue;
        }

        x = y = z = w = 0;
        for (k = 0; k < size; k++) {
        if (i - half + k < 0 || i - half + k >= height)
            continue;

        s = (uint32_t *) (dst + (i - half + k) * dst_stride);
        p = s[j];

        x += ((p >> 24) & 0xff) * kernel[k];
        y += ((p >> 16) & 0xff) * kernel[k];
        z += ((p >>  8) & 0xff) * kernel[k];
        w += ((p >>  0) & 0xff) * kernel[k];
        }
        d[j] = (x / a << 24) | (y / a << 16) | (z / a << 8) | w / a;
    }
    }

    cairo_surface_destroy (tmp);
    cairo_surface_mark_dirty (surface);
}
*/



/*
 * The stack blur algorithm was invented by Mario Klingemann <mario@quasimondo.com>
 * http://incubator.quasimondo.com/processing/fast_blur_deluxe.php
 *
 * Compared to Mario's original source code, the lookup table is removed as the benefit
 * doesn't worth the memory usage in case of large radiuses. Also, the following code adds
 * alpha channel support and different radius for vertical and horizontal directions.
 */
/*
static void
stack_blur (cairo_surface_t *input, cairo_surface_t *output, int rx, int ry)
{
    guint32 *input_pixels;
    guint32 *output_pixels;
    int w, h, wm, hm, wh, div;
    int *r, *g, *b, *a;

    int rsum,gsum,bsum,asum,x,y,i,p,yp,yi,yw;
    int *vmin;
    int divsum;

    yw=yi=0;

    int *stack;
    int stackpointer;
    int stackstart;
    int *sir;
    int rbs;
    int r1;
    int routsum,goutsum,boutsum, aoutsum;
    int rinsum,ginsum,binsum, ainsum;
    int rowstride;

    g_return_if_fail (rx > 0 || ry > 0);

    input_pixels = (guint32 *) cairo_image_surface_get_data (input);
    output_pixels = (guint32 *) cairo_image_surface_get_data (output);
    rowstride = cairo_image_surface_get_stride (input);
    w = cairo_image_surface_get_width (input);
    h = cairo_image_surface_get_height (input);

    g_return_if_fail (cairo_image_surface_get_width (output) == w);
    g_return_if_fail (cairo_image_surface_get_height (output) == h);
    g_return_if_fail (cairo_image_surface_get_stride (output) == rowstride);

    wm = w - 1;
    hm = h - 1;
    wh = w * h;
    
    r = g_new (int, wh);
    g = g_new (int, wh);
    b = g_new (int, wh);
    a = g_new (int, wh);

    div = 2 * rx + 1;
    divsum = (div + 1) >> 1; 
    divsum *= divsum;
    stack = g_new (int, div * 4);
    r1 = rx + 1;

    vmin = g_new (int, w);
    for (y =0; y < h; y++) {
	rinsum=ginsum=binsum=ainsum=routsum=goutsum=boutsum=aoutsum=rsum=gsum=bsum=asum=0;
	yi = y * rowstride / 4;

	for (i = -rx; i <= rx; i++) {
	    p = input_pixels [yi + MIN (wm, MAX (i, 0))];
	    sir = &stack [4 * (i + rx)];
	    sir[0]=(p & 0x00ff0000)>>16;
	    sir[1]=(p & 0x0000ff00)>>8;
	    sir[2]=(p & 0x000000ff);
	    sir[3]=(p & 0xff0000ff)>>24;
	    rbs= r1 - ABS (i);
	    rsum+=sir[0]*rbs;
	    gsum+=sir[1]*rbs;
	    bsum+=sir[2]*rbs;
	    asum+=sir[3]*rbs;
	    if (i>0){
		rinsum+=sir[0];
		ginsum+=sir[1];
		binsum+=sir[2];
		ainsum+=sir[3];
	    } else {
		routsum+=sir[0];
		goutsum+=sir[1];
		boutsum+=sir[2];
		aoutsum+=sir[3];
	    }
	}
	stackpointer=rx;

	for (x=0;x<w;x++){

	    r[yi] = rsum / divsum;
	    g[yi] = gsum / divsum;
	    b[yi] = bsum / divsum;
	    a[yi] = asum / divsum;
	    rsum-=routsum;
	    gsum-=goutsum;
	    bsum-=boutsum;
	    asum-=aoutsum;

	    stackstart=stackpointer-rx+div;
	    sir = &stack [4 * (stackstart % div)];

	    routsum-=sir[0];
	    goutsum-=sir[1];
	    boutsum-=sir[2];
	    aoutsum-=sir[3];

	    if(y==0){
		vmin[x]=MIN (x + rx + 1, wm);
	    }
	    p = input_pixels [yw + vmin[x]];

	    sir[0]=(p & 0x00ff0000)>>16;
	    sir[1]=(p & 0x0000ff00)>>8;
	    sir[2]=(p & 0x000000ff);
	    sir[3]=(p & 0xff000000)>>24;

	    rinsum+=sir[0];
	    ginsum+=sir[1];
	    binsum+=sir[2];
	    ainsum+=sir[3];

	    rsum+=rinsum;
	    gsum+=ginsum;
	    bsum+=binsum;
	    asum+=ainsum;

	    stackpointer = (stackpointer + 1) % div;
	    sir = &stack [4 * ((stackpointer) % div)];

	    routsum+=sir[0];
	    goutsum+=sir[1];
	    boutsum+=sir[2];
	    aoutsum+=sir[3];

	    rinsum-=sir[0];
	    ginsum-=sir[1];
	    binsum-=sir[2];
	    ainsum-=sir[3];

	    yi++;
	}
	yw+=w;
    }
    g_free (vmin);
    g_free (stack);

    div = 2 * ry + 1;
    divsum = (div + 1) >> 1; 
    divsum *= divsum;

    stack = g_new0 (int, div * 4);
    r1 = ry + 1;

    vmin = g_new0 (int, h);
    for (x=0;x<w;x++){
	rinsum=ginsum=binsum=ainsum=routsum=goutsum=boutsum=aoutsum=rsum=gsum=bsum=asum=0;
	yp=-ry*w;
	for(i=-ry;i<=ry;i++){
	    yi= MAX(0,yp)+x;

	    sir = &stack [4 * (i + ry)];

	    sir[0]=r[yi];
	    sir[1]=g[yi];
	    sir[2]=b[yi];
	    sir[3]=a[yi];

	    rbs=r1 - ABS(i);

	    rsum+=r[yi]*rbs;
	    gsum+=g[yi]*rbs;
	    bsum+=b[yi]*rbs;
	    asum+=a[yi]*rbs;

	    if (i>0){
		rinsum+=sir[0];
		ginsum+=sir[1];
		binsum+=sir[2];
		ainsum+=sir[3];
	    } else {
		routsum+=sir[0];
		goutsum+=sir[1];
		boutsum+=sir[2];
		aoutsum+=sir[3];
	    }

	    if(i<hm){
		yp+=w;
	    }
	}
	yi=x;
	stackpointer=ry;
	for (y=0;y<h;y++){
	    output_pixels [yi] = 
		((asum / divsum) << 24) |
		((rsum / divsum) << 16) |
		((gsum / divsum) << 8) |
		(bsum / divsum);
	    rsum-=routsum;
	    gsum-=goutsum;
	    bsum-=boutsum;
	    asum-=aoutsum;

	    stackstart=stackpointer-ry+div;
	    sir = &stack [4 * (stackstart % div)];

	    routsum-=sir[0];
	    goutsum-=sir[1];
	    boutsum-=sir[2];
	    aoutsum-=sir[3];

	    if(x==0) {
		vmin[y] = MIN(y+r1,hm)*w;
	    }
	    p=x+vmin[y];

	    sir[0]=r[p];
	    sir[1]=g[p];
	    sir[2]=b[p];
	    sir[3]=a[p];

	    rinsum+=sir[0];
	    ginsum+=sir[1];
	    binsum+=sir[2];
	    ainsum+=sir[3];

	    rsum+=rinsum;
	    gsum+=ginsum;
	    bsum+=binsum;
	    asum+=ainsum;

	    stackpointer=(stackpointer+1)%div;
	    sir = &stack[4 * stackpointer];

	    routsum+=sir[0];
	    goutsum+=sir[1];
	    boutsum+=sir[2];
	    aoutsum+=sir[3];

	    rinsum-=sir[0];
	    ginsum-=sir[1];
	    binsum-=sir[2];
	    ainsum-=sir[3];

	    yi+= rowstride / 4;
	}
    }
    g_free (vmin);
    g_free (stack);

    g_free (r);
    g_free (g);
    g_free (b);
    g_free (a);
}

*/